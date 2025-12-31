using AgentFramework.Core.Configuration;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace AgentFramework.Core.Agents
{
    /// <summary>
    /// Provides extension methods for <see cref="AIAgent"/> to simplify common agent operations.
    /// </summary>
    public static class AIAgentExtensions
    {
        private static Dictionary<AgentThread, string> _threadKeys = new Dictionary<AgentThread, string>();

        /// <summary>
        /// Streams an agent's response to a text prompt asynchronously.
        /// </summary>
        /// <param name="agent">The AI agent to run.</param>
        /// <param name="prompt">The user prompt text.</param>
        /// <param name="onUpdate">Callback invoked for each streaming update.</param>
        /// <param name="agentThread">Optional thread context for the conversation.</param>
        /// <param name="options">Optional configuration for the agent run.</param>
        /// <param name="cancellationToken">Cancellation token to stop the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task StreamResponseAsync(
                                    this AIAgent agent,
                                    string prompt,
                                    Action<AgentRunResponseUpdate?> onUpdate,
                                    AgentThread agentThread = default!,
                                    AgentRunOptions? options = null,
                                    CancellationToken cancellationToken = default)
        {
            await StreamResponseAsync(agent, new ChatMessage(ChatRole.User, prompt), onUpdate, agentThread, options, cancellationToken);
        }

        /// <summary>
        /// Streams an agent's response to a single chat message asynchronously.
        /// </summary>
        /// <param name="agent">The AI agent to run.</param>
        /// <param name="message">The chat message to process.</param>
        /// <param name="onUpdate">Callback invoked for each streaming update.</param>
        /// <param name="thread">Optional thread context for the conversation.</param>
        /// <param name="options">Optional configuration for the agent run.</param>
        /// <param name="cancellationToken">Cancellation token to stop the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task StreamResponseAsync(
                        this AIAgent agent,
                        ChatMessage message,
                        Action<AgentRunResponseUpdate?> onUpdate,
                        AgentThread? thread = null,
                        AgentRunOptions? options = null,
                        CancellationToken cancellationToken = default)
        {
            await StreamResponseAsync(agent, new[] { message }, onUpdate, thread, options, cancellationToken);
        }

        /// <summary>
        /// Streams an agent's response to multiple chat messages asynchronously.
        /// </summary>
        /// <param name="agent">The AI agent to run.</param>
        /// <param name="messages">The collection of chat messages to process.</param>
        /// <param name="onUpdate">Callback invoked for each streaming update.</param>
        /// <param name="thread">Optional thread context for the conversation.</param>
        /// <param name="options">Optional configuration for the agent run.</param>
        /// <param name="cancellationToken">Cancellation token to stop the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown when agent, messages, or onUpdate is null.</exception>
        public static async Task StreamResponseAsync(
                        this AIAgent agent,
                        IEnumerable<ChatMessage> messages,
                        Action<AgentRunResponseUpdate?> onUpdate,
                        AgentThread? thread = null,
                        AgentRunOptions? options = null,
                        CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(agent);
            ArgumentNullException.ThrowIfNull(messages);
            ArgumentNullException.ThrowIfNull(onUpdate);
            await foreach (var update in agent.RunStreamingAsync(messages, thread, options, cancellationToken: cancellationToken)
                                             .WithCancellation(cancellationToken)
                                             .ConfigureAwait(false))
            {
                // If you want to guard against callback exceptions killing the stream,
                // wrap this in try/catch and decide what to do.
                onUpdate(update);
            }
        }

        /// <summary>
        /// Streams an agent's response to a text prompt synchronously.
        /// </summary>
        /// <param name="agent">The AI agent to run.</param>
        /// <param name="prompt">The user prompt text.</param>
        /// <param name="onUpdate">Callback invoked for each streaming update.</param>
        /// <param name="agentThread">Optional thread context for the conversation.</param>
        /// <param name="options">Optional configuration for the agent run.</param>
        /// <param name="cancellationToken">Cancellation token to stop the operation.</param>
        public static void StreamResponse(
            this AIAgent agent,
            string prompt,
            Action<AgentRunResponseUpdate?> onUpdate,
            AgentThread agentThread = default!,
            AgentRunOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            StreamResponseAsync(agent, prompt, onUpdate, agentThread, options, cancellationToken)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Streams an agent's response to a single chat message synchronously.
        /// </summary>
        /// <param name="agent">The AI agent to run.</param>
        /// <param name="message">The chat message to process.</param>
        /// <param name="onUpdate">Callback invoked for each streaming update.</param>
        /// <param name="thread">Optional thread context for the conversation.</param>
        /// <param name="options">Optional configuration for the agent run.</param>
        /// <param name="cancellationToken">Cancellation token to stop the operation.</param>
        public static void StreamResponse(
            this AIAgent agent,
            ChatMessage message,
            Action<AgentRunResponseUpdate?> onUpdate,
            AgentThread? thread = null,
            AgentRunOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            StreamResponseAsync(agent, message, onUpdate, thread, options, cancellationToken)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Streams an agent's response to multiple chat messages synchronously.
        /// </summary>
        /// <param name="agent">The AI agent to run.</param>
        /// <param name="messages">The collection of chat messages to process.</param>
        /// <param name="onUpdate">Callback invoked for each streaming update.</param>
        /// <param name="thread">Optional thread context for the conversation.</param>
        /// <param name="options">Optional configuration for the agent run.</param>
        /// <param name="cancellationToken">Cancellation token to stop the operation.</param>
        public static void StreamResponse(
            this AIAgent agent,
            IEnumerable<ChatMessage> messages,
            Action<AgentRunResponseUpdate?> onUpdate,
            AgentThread? thread = null,
            AgentRunOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            StreamResponseAsync(agent, messages, onUpdate, thread, options, cancellationToken)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Retrieves the configuration associated with an AI agent.
        /// </summary>
        /// <param name="agent">The AI agent.</param>
        /// <returns>The agent's configuration.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the agent configuration is not found. This typically occurs when the agent
        /// was not created using the <see cref="AgentFactory"/>>.
        /// </exception>
        public static AgentConfiguration GetConfiguration(this AIAgent agent)
        {
            var agentConfig = AgentFactoryMemory.GetConfiguration(agent.Id);
            if (agentConfig is null)
            {
                throw new InvalidOperationException(
    $"Agent configuration not found for agent '{agent.Id}'. " +
    $"Configurations are only available for agents created through the {nameof(AgentFactory)}. " +
    "Ensure the agent was instantiated using AgentFactory methods.");
            }
            return agentConfig;
        }
    }
}
