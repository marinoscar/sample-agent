using AgentFramework.Core.Configuration;
using AgentFramework.Core.Data;
using AgentFramework.Core.Middleware;
using Microsoft.Agents.AI;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        private readonly Func<IAgentStore> _agentMessageStoreFactory;
        private static bool hasStoreBeenInitialized = false;

        public AgentFactory(Func<IAgentStore>? agentMessageStoreFactory = null, ILoggerFactory? loggerFactory = null)
        {
            _logger = loggerFactory;
            _agentMessageStoreFactory = agentMessageStoreFactory!;

            _agentMessageStoreFactory ??= (() => new AgentStoreService(() => new SqliteAgentMessageContext()));
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

        public async Task<AIAgent> CreateAgentAsync(string agentId, CancellationToken ct = default)
        {
            var config = await _agentMessageStoreFactory().GetAgentConfigurationByIdAsync(agentId, ct);
            return CreateAgent(config);
        }

        public AIAgent CreateAgent(string agentId)
        {
            return CreateAgentAsync(agentId).GetAwaiter().GetResult();
        }

        public async Task<AgentConfiguration> PersistConfigurationAsync(AgentConfiguration config, CancellationToken ct = default)
        {
            var store = _agentMessageStoreFactory();
            await store.AddOrUpdateAsync(config, ct);
            return config;
        }

        private AIAgent ApplyMiddleware(AIAgent innerAgent, AgentMiddlewareOptions middlewareOptions)
        {
            var agentBuilder = innerAgent.AsBuilder();

            if (middlewareOptions.FunctionCalling != null)
            {
                agentBuilder = agentBuilder.Use(middlewareOptions.FunctionCalling);
            }

            if (middlewareOptions.Run != null || middlewareOptions.Streaming != null)
            {
                agentBuilder = agentBuilder.Use(middlewareOptions.Run, middlewareOptions.Streaming);
            }

            return agentBuilder.Build();
        }

        public AIAgent CreateAgent(AgentConfiguration agentSettings, AgentMiddlewareOptions? middlewareOptions = null)
        {
            if(agentSettings == null) throw new ArgumentNullException(nameof(agentSettings));
            if(string.IsNullOrEmpty(agentSettings.Provider)) throw new ArgumentException("Agent provider must be specified.", nameof(agentSettings.Provider));

            return agentSettings.Provider?.ToLowerInvariant().Trim() switch
            {
                "openai" => CreateOpenAIAgent(agentSettings, middlewareOptions),
                "azureopenai" => CreateAzureOpenAIAgent(agentSettings, middlewareOptions),
                "anthropic" => CreateAnthropicAIAgent(agentSettings, middlewareOptions),
                "gemini" => CreateGeminiAIAgent(agentSettings, middlewareOptions),
                _ => throw new NotSupportedException($"The provider '{agentSettings.Provider}' is not supported."),
            };
        }

        public AIAgent CreateOpenAIAgent(AgentConfiguration agentSettings, AgentMiddlewareOptions? middlewareOptions = null)
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

            return middlewareOptions != null ? ApplyMiddleware(innerAgent, middlewareOptions) : innerAgent;
        }

        public AIAgent CreateAzureOpenAIAgent(AgentConfiguration agentSettings, AgentMiddlewareOptions? middlewareOptions = null)
        {
            throw new NotImplementedException();
        }

        public AIAgent CreateAnthropicAIAgent(AgentConfiguration agentSettings, AgentMiddlewareOptions? middlewareOptions = null)
        {
            throw new NotImplementedException();
        }

        public AIAgent CreateGeminiAIAgent(AgentConfiguration agentSettings, AgentMiddlewareOptions? middlewareOptions = null)
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
                return new DbAgentChatMessageStore(_agentMessageStoreFactory, agentSettings.Id, agentSettings.Name,context.SerializedState, context.JsonSerializerOptions);
            };
        }
    }
}
