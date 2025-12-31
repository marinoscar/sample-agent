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
    /// Initializes research queue (threads) from the ResearchBrief and emits the first work item.
    /// </summary>
    public sealed class InitializeResearchQueueExecutor : Executor<ClarifierAgentResponse, ThreadWorkItem>
    {
        public const string ResearchScope = "ResearchState";
        public const string QueueScope = "QueueState";

        public InitializeResearchQueueExecutor() : base(nameof(InitializeResearchQueueExecutor)) { }

        public override async ValueTask<ThreadWorkItem> HandleAsync(
            ClarifierAgentResponse message,
            IWorkflowContext context,
            CancellationToken cancellationToken = default)
        {
            if (message.NeedsClarification)
                throw new InvalidOperationException("InitializeResearchQueueExecutor must only run when clarification is NOT needed.");

            if (message.Brief is null)
                throw new InvalidOperationException("ClarifierAgentResponse.Brief was null when clarification was not needed.");

            // Persist the brief for downstream executors.
            await context.QueueStateUpdateAsync("brief", message.Brief, scopeName: ResearchScope);

            // Initialize queue with thread IDs.
            var threadIds = message.Brief.Threads.Select(t => t.Id).ToList();
            await context.QueueStateUpdateAsync("thread_ids", threadIds, scopeName: QueueScope);

            // Emit first thread work item (or done if none).
            var next = message.Brief.Threads.FirstOrDefault();
            return next is null
                ? ThreadWorkItem.Done()
                : ThreadWorkItem.Next(next.Id);
        }
    }
}
