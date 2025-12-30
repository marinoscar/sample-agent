using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AgentFramework.Core.Agents
{
    /// <summary>
    /// Provides the delegates used to configure agent middleware components.
    /// </summary>
    public class AgentMiddlewareOptions
    {
        /// <summary>
        /// A delegate that processes function invocations. The delegate receives the <see cref="AIAgent"/> instance,
        /// the function invocation context, and a continuation delegate representing the next callback in the pipeline.
        /// It returns a task representing the result of the function invocation.
        /// </summary>
        /// <remarks>
        /// <seealso cref="FunctionInvocationDelegatingAgentBuilderExtensions.Use(AIAgentBuilder, Func{AIAgent, FunctionInvocationContext, Func{FunctionInvocationContext, CancellationToken, ValueTask{object?}}, CancellationToken, ValueTask{object?}})"/>
        /// </remarks>
        public Func<AIAgent, FunctionInvocationContext, Func<FunctionInvocationContext, CancellationToken, ValueTask<object?>>, CancellationToken, ValueTask<object?>>? FunctionCalling { get; set; }
        /// <summary>
        /// A delegate that provides the implementation for <see cref="AIAgent.RunAsync(IEnumerable{ChatMessage}, AgentThread?, AgentRunOptions?, CancellationToken)"/>
        /// </summary>
        /// <remarks>
        /// <seealso cref="AIAgentBuilder.Use(Func{IEnumerable{ChatMessage}, AgentThread?, AgentRunOptions?, AIAgent, CancellationToken, Task{AgentRunResponse}}?, Func{IEnumerable{ChatMessage}, AgentThread?, AgentRunOptions?, AIAgent, CancellationToken, IAsyncEnumerable{AgentRunResponseUpdate}}?)"/>
        /// </remarks>
        public Func<IEnumerable<ChatMessage>, AgentThread?, AgentRunOptions?, AIAgent, CancellationToken, Task<AgentRunResponse>>? Run { get; set; }
        /// <summary>
        /// A delegate that provides the implementation for <see cref="AIAgent.RunStreamingAsync(IEnumerable{ChatMessage}, AgentThread?, AgentRunOptions?, CancellationToken)"/>
        /// </summary>
        /// /// <remarks>
        /// <seealso cref="AIAgentBuilder.Use(Func{IEnumerable{ChatMessage}, AgentThread?, AgentRunOptions?, AIAgent, CancellationToken, Task{AgentRunResponse}}?, Func{IEnumerable{ChatMessage}, AgentThread?, AgentRunOptions?, AIAgent, CancellationToken, IAsyncEnumerable{AgentRunResponseUpdate}}?)"/>
        /// </remarks>
        public Func<IEnumerable<ChatMessage>, AgentThread?, AgentRunOptions?, AIAgent, CancellationToken, IAsyncEnumerable<AgentRunResponseUpdate>>? Streaming { get; set; }
    }
}
