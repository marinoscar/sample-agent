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
    /// Final executor: yields the report to the caller.
    /// </summary>
    public sealed class FinalizeExecutor : Executor<SynthesizerAgentResponse>
    {
        public FinalizeExecutor() : base(nameof(FinalizeExecutor)) { }

        public override async ValueTask HandleAsync(
            SynthesizerAgentResponse message,
            IWorkflowContext context,
            CancellationToken cancellationToken = default)
        {
            await context.YieldOutputAsync(message.DraftReport);
        }
    }
}
