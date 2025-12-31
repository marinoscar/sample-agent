using DeepResearchAgent.DTO;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.Executors
{
    /// <summary>
    /// Entry executor: converts an inbound ChatMessage into a ResearchTopicRequest and stores it in workflow state.
    /// </summary>
    public sealed class IntakeExecutor : Executor<ChatMessage, ResearchTopicRequest>
    {
        public const string ResearchScope = "ResearchState";

        public IntakeExecutor() : base(nameof(IntakeExecutor)) { }

        public override async ValueTask<ResearchTopicRequest> HandleAsync(
            ChatMessage message,
            IWorkflowContext context,
            CancellationToken cancellationToken = default)
        {
            var req = new ResearchTopicRequest
            {
                Topic = message.Text
            };

            await context.QueueStateUpdateAsync(
                key: "request",
                value: req,
                scopeName: ResearchScope);

            return req;
        }
    }
}
