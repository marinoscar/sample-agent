using AgentFramework.Core.Configuration;
using AgentFramework.Core.Data;
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
        private readonly Func<AgentChatMessageStore>? _chatMessageStoreFactory;

        public AgentFactory(Func<AgentChatMessageStore>? chatMessageStoreFactory = null, ILoggerFactory? loggerFactory = null)
        {
            _logger = loggerFactory;
            _chatMessageStoreFactory = chatMessageStoreFactory ?? GetStoreFactory();
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
            var agent = responsesClient.CreateAIAgent(options: new ChatClientAgentOptions()
            {
                Id = agentSettings.Id,
                Name = agentSettings.Name,
                Description = agentSettings.Description,
                ChatOptions = new ChatOptions()
                {
                    Instructions = agentSettings.Instructions,
                    ModelId = agentSettings.Model,
                    Temperature = (float)(agentSettings.Temperature),
                    ResponseFormat = agentSettings.GetResponseFormat(),
                    ToolMode = agentSettings.GetToolMode(),
                    Tools = new AgentToolFactory().GetTools(agentSettings.ToolList),
                },
                ChatMessageStoreFactory = GetStore(agentSettings),
            }, loggerFactory: _logger);
            
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
            return (context) =>
            {
                var store = _chatMessageStoreFactory!();
                
                // Extract thread ID from serialized state or generate new one
                var threadId = context.SerializedState.ValueKind is JsonValueKind.String
                    ? context.SerializedState.Deserialize<string>() ?? Guid.NewGuid().ToString("N")
                    : Guid.NewGuid().ToString("N");

                store.AgentInfo = new AgentChatMetadata
                {
                    AgentId = agentSettings.Id!,
                    AgentName = agentSettings.Name!,
                    ThreadId = threadId,
                };
                
                return store;
            };
        }

        private static Func<AgentChatMessageStore> GetStoreFactory() {

            return () =>
            new SqlChatMessageStore(new AgentMessageStoreService(() => new SqliteAgentMessageContext()));

         }

    }
}
