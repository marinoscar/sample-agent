using AgentFramework.Core.Configuration;
using AgentFramework.Core.Data;
using AgentFramework.Core.Middleware;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Microsoft.Agents.AI.ChatClientAgentOptions;

namespace AgentFramework.Core.Agents
{
    public class AgentFactory
    {

        private readonly ILoggerFactory? _logger;
        private readonly Func<IAgentMessageStore> _agentMessageStoreFactory;
        private static bool hasStoreBeenInitialized = false;

        public AgentFactory(Func<IAgentMessageStore>? agentMessageStoreFactory = null, ILoggerFactory? loggerFactory = null)
        {
            _logger = loggerFactory;
            _agentMessageStoreFactory = agentMessageStoreFactory ?? throw new ArgumentNullException(nameof(agentMessageStoreFactory));
        }

        public async Task EnsureStoreIsReadyAsync(CancellationToken ct = default)
        {
            if(hasStoreBeenInitialized) return;
            var store = _agentMessageStoreFactory();
            await store.EnsureStoreIsReadyAsync(ct);
            hasStoreBeenInitialized = true;
        }

        public AIAgent CreateAgent(AgentConfiguration agentSettings)
        {
            return agentSettings.Provider?.ToLowerInvariant().Trim() switch
            {
                "openai" => CreateOpenAIAgent(agentSettings),
                "azureopenai" => CreateAzureOpenAIAgent(agentSettings),
                "anthropic" => CreateAnthropicAIAgent(agentSettings),
                "gemini" => CreateGeminiAIAgent(agentSettings),
                _ => throw new NotSupportedException($"The provider '{agentSettings.Provider}' is not supported."),
            };
        }

        public AIAgent CreateOpenAIAgent(AgentConfiguration agentSettings)
        {
            var responsesClient = CreateOpenAIResponsesClient(agentSettings);
            var innerAgent = responsesClient.CreateAIAgent(options: new ChatClientAgentOptions()
            {
                Id = agentSettings.Id,
                Name = agentSettings.Name,
                Description = agentSettings.Description,
                ChatOptions = new ChatOptions()
                {
                    Instructions = agentSettings.Instructions,
                    ModelId = agentSettings.Model,
                    Temperature = agentSettings.Temperature,
                    ResponseFormat = agentSettings.GetResponseFormat(),
                    ToolMode = agentSettings.GetToolMode(),
                    Tools = new AgentToolFactory().GetTools(agentSettings.ToolList),
                    //Using this line to fix the issue described here
                    //https://github.com/microsoft/agent-framework/issues/2912#issuecomment-3679548491
                    RawRepresentationFactory = _ => new CreateResponseOptions() { StoredOutputEnabled = false },
                },
                ChatMessageStoreFactory = GetStore(agentSettings),
            }, loggerFactory: _logger);
            var agent = innerAgent.AsBuilder()
                          .Use(InspectMiddleware.FunctionCallingMiddleware)
                          .Use(InspectMiddleware.RunInspectionAsync, InspectMiddleware.RunStreamingInspectionAsync)
                          .Build();
            return agent;
        }

        public AIAgent CreateAzureOpenAIAgent(AgentConfiguration agentSettings)
        {
            throw new NotImplementedException();
        }

        public AIAgent CreateAnthropicAIAgent(AgentConfiguration agentSettings)
        {
            throw new NotImplementedException();
        }

        public AIAgent CreateGeminiAIAgent(AgentConfiguration agentSettings)
        {
            throw new NotImplementedException();
        }

        private ResponsesClient CreateOpenAIResponsesClient(AgentConfiguration agentSettings)
        {
            var key = new CredentialFactory().GetFromProvider(agentSettings.Provider);
            var client = new OpenAIClient(key.ApiKey);
            var responses = client.GetResponsesClient(agentSettings.Model);
            return responses;
        }

        private Func<ChatMessageStoreFactoryContext, ChatMessageStore> GetStore(AgentConfiguration agentSettings)
        {
            if (!agentSettings.PersistConversation)
            {
                return (c) => 
                    new InMemoryChatMessageStore(c.SerializedState, c.JsonSerializerOptions);
            }
            return (context) =>
            {
                return new SqlChatMessageStore(_agentMessageStoreFactory, agentSettings.Id, agentSettings.Name,context.SerializedState, context.JsonSerializerOptions);
            };
        }
    }
}
