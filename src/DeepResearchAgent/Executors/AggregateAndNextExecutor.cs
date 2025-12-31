using DeepResearchAgent.DTO;
using Microsoft.Agents.AI.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.Executors
{
    /// <summary>
    /// Aggregates the findings for the completed thread, advances the queue, and emits the next ThreadWorkItem (or Done).
    /// </summary>
    public sealed class AggregateAndNextExecutor : Executor<WebResearchAgentResponse, ThreadWorkItem>
    {
        public const string FindingsScope = "FindingsState";
        public const string QueueScope = "QueueState";
        public const string ResearchScope = "ResearchState";

        public AggregateAndNextExecutor() : base(nameof(AggregateAndNextExecutor)) { }

        public override async ValueTask<ThreadWorkItem> HandleAsync(
            WebResearchAgentResponse message,
            IWorkflowContext context,
            CancellationToken cancellationToken = default)
        {
            // Store findings keyed by ThreadId.
            await context.QueueStateUpdateAsync(
                key: message.ThreadId,
                value: message,
                scopeName: FindingsScope);

            // Pop current thread id from queue.
            var queue = await context.ReadStateAsync<List<string>>("thread_ids", scopeName: QueueScope) ?? new();
            if (queue.Count > 0 && queue[0] == message.ThreadId)
            {
                queue.RemoveAt(0);
                await context.QueueStateUpdateAsync("thread_ids", queue, scopeName: QueueScope);
            }

            // Determine next thread.
            if (queue.Count == 0)
                return ThreadWorkItem.Done();

            return ThreadWorkItem.Next(queue[0]);
        }
    }
}
