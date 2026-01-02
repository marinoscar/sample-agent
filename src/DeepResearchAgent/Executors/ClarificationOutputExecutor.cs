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
    /// If clarification is needed, yield the questions and end the workflow (caller resumes later with answers).
    /// </summary>
    public sealed class ClarificationOutputExecutor : Executor<ClarifierAgentResponse>
    {
        public ClarificationOutputExecutor() : base(nameof(ClarificationOutputExecutor)) { }

        public override async ValueTask HandleAsync(
            ClarifierAgentResponse message,
            IWorkflowContext context,
            CancellationToken cancellationToken = default)
        {
            await context.YieldOutputAsync(new
            {
                needs_clarification = true,
                questions = message.Questions,
                //assumptions_if_no_answer = message.AssumptionsIfNoAnswer
            });
        }
    }
}
