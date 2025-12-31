using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.DTO
{
    public sealed class PlannerAgentResponse
    {
        /// <summary>
        /// The finalized research plan for execution.
        /// The Orchestrator uses this to dispatch work to the WebResearchAgent.
        /// </summary>
        public required ResearchPlanDto Plan { get; init; }

        /// <summary>
        /// Notes about planning choices (e.g., why certain sub-questions were prioritized).
        /// Useful for debugging or transparency.
        /// </summary>
        public List<string> Notes { get; init; } = new();
    }
}
