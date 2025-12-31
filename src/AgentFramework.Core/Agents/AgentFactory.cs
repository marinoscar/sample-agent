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
    /// <summary>
    /// Factory class responsible for creating and managing AI agents with various provider implementations.
    /// Supports OpenAI, Azure OpenAI, Anthropic, and Gemini providers.
    /// </summary>
    public class AgentFactory
    {
        private readonly ILoggerFactory? _loggerFactory;
        private readonly ILogger<AgentFactory>? _logger;
        private readonly Func<IAgentStore> _agentMessageStoreFactory;
        private static bool hasStoreBeenInitialized = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentFactory"/> class.
        /// </summary>
        /// <param name="agentMessageStoreFactory">Optional factory function for creating agent store instances. Defaults to SQLite-based store if not provided.</param>
        /// <param name="loggerFactory">Optional logger factory for creating loggers. If null, logging will be disabled.</param>
        public AgentFactory(Func<IAgentStore>? agentMessageStoreFactory = null, ILoggerFactory? loggerFactory = null)
        {
            _loggerFactory = loggerFactory;
            _logger = loggerFactory?.CreateLogger<AgentFactory>();
            _agentMessageStoreFactory = agentMessageStoreFactory!;

            _agentMessageStoreFactory ??= (() => new AgentStoreService(() => new SqliteAgentContext()));
        }

        /// <summary>
        /// Ensures the agent store is initialized and ready for use.
        /// This operation is idempotent - subsequent calls will return immediately if already initialized.
        /// </summary>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous initialization operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the store initialization fails.</exception>
        /// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
        public async Task EnsureStoreIsReadyAsync(CancellationToken ct = default)
        {
            try
            {
                if (hasStoreBeenInitialized) return;
                var store = _agentMessageStoreFactory();
                await store.EnsureStoreIsReadyAsync(ct);
                hasStoreBeenInitialized = true;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to initialize agent store.");
                throw new InvalidOperationException("Failed to initialize agent store. See inner exception for details.", ex);
            }
        }

        /// <summary>
        /// Creates an AI agent by loading its configuration from the agent store using the specified agent ID.
        /// </summary>
        /// <param name="agentId">The unique identifier of the agent to create.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation and contains the configured <see cref="AIAgent"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when the agent configuration is invalid.</exception>
        /// <exception cref="NotSupportedException">Thrown when the specified provider is not supported.</exception>
        public async Task<AIAgent> CreateAgentAsync(string agentId, CancellationToken ct = default)
        {
            var config = await _agentMessageStoreFactory().GetAgentConfigurationByIdAsync(agentId, ct);
            return CreateAgent(config);
        }

        /// <summary>
        /// Creates an AI agent synchronously by loading its configuration from the agent store using the specified agent ID.
        /// This is a blocking operation that waits for the asynchronous operation to complete.
        /// </summary>
        /// <param name="agentId">The unique identifier of the agent to create.</param>
        /// <returns>A configured <see cref="AIAgent"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when the agent configuration is invalid.</exception>
        /// <exception cref="NotSupportedException">Thrown when the specified provider is not supported.</exception>
        public AIAgent CreateAgent(string agentId)
        {
            return CreateAgentAsync(agentId).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Persists or updates an agent configuration in the agent store.
        /// </summary>
        /// <param name="config">The agent configuration to persist.</param>
        /// <param name="ct">Cancellation token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation and contains the persisted <see cref="AgentConfiguration"/>.</returns>
        public async Task<AgentConfiguration> PersistConfigurationAsync(AgentConfiguration config, CancellationToken ct = default)
        {
            var store = _agentMessageStoreFactory();
            await store.AddOrUpdateAsync(config, ct);
            return config;
        }

        /// <summary>
        /// Creates an AI agent with custom middleware applied based on the provided configuration.
        /// Middleware can be used to intercept and modify agent behavior for function calling, execution, and streaming.
        /// </summary>
        /// <param name="agentSettings">Configuration settings for the agent including provider, model, and behavior settings.</param>
        /// <param name="middlewareOptions">Optional middleware configuration to apply custom behavior to the agent.</param>
        /// <returns>A configured <see cref="AIAgent"/> instance with middleware applied if specified.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="agentSettings"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the Provider property is missing from the configuration.</exception>
        /// <exception cref="NotSupportedException">Thrown when the specified provider is not supported.</exception>
        public AIAgent CreateAgent(AgentConfiguration agentSettings, AgentMiddlewareOptions? middlewareOptions = null)
        {
            if(agentSettings == null) throw new ArgumentNullException(nameof(agentSettings));
            if(string.IsNullOrEmpty(agentSettings.Provider)) throw new ArgumentException("Agent provider must be specified.", nameof(agentSettings.Provider));
            if(string.IsNullOrEmpty(agentSettings.Id)) throw new ArgumentException("Agent Id must be specified.", nameof(agentSettings.Id));

            var agent = agentSettings.Provider?.ToLowerInvariant().Trim() switch
            {
                "openai" => CreateOpenAIAgent(agentSettings, middlewareOptions),
                "azureopenai" => CreateAzureOpenAIAgent(agentSettings, middlewareOptions),
                "anthropic" => CreateAnthropicAIAgent(agentSettings, middlewareOptions),
                "gemini" => CreateGeminiAIAgent(agentSettings, middlewareOptions),
                _ => throw new NotSupportedException($"The provider '{agentSettings.Provider}' is not supported."),
            };
            AgentFactoryMemory.SetConfiguration(agent.Id, agentSettings);
            return agent;
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

        private AIAgent CreateOpenAIAgent(AgentConfiguration agentSettings, AgentMiddlewareOptions? middlewareOptions = null)
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
            }, loggerFactory: _loggerFactory);

            return middlewareOptions != null ? ApplyMiddleware(innerAgent, middlewareOptions) : innerAgent;
        }

        private AIAgent CreateAzureOpenAIAgent(AgentConfiguration agentSettings, AgentMiddlewareOptions? middlewareOptions = null)
        {
            throw new NotImplementedException();
        }

        private AIAgent CreateAnthropicAIAgent(AgentConfiguration agentSettings, AgentMiddlewareOptions? middlewareOptions = null)
        {
            throw new NotImplementedException();
        }

        private AIAgent CreateGeminiAIAgent(AgentConfiguration agentSettings, AgentMiddlewareOptions? middlewareOptions = null)
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
