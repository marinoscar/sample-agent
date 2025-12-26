using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Agents
{
    public static class AIAgentExtensions
    {

        private static Dictionary<AgentThread, string> _threadKeys = new Dictionary<AgentThread, string>();

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
    }
}
