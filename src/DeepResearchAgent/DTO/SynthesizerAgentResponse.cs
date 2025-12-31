using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.DTO
{
    public sealed class SynthesizerAgentResponse
    {
        /// <summary>
        /// The drafted report ready for the Orchestrator to finalize and present to the user.
        /// </summary>
        public required DraftReportDto DraftReport { get; init; }

        /// <summary>
        /// Claims that were excluded due to insufficient evidence or weak sources.
        /// This keeps your final output honest and auditable.
        /// </summary>
        public List<string> ExcludedClaims { get; init; } = new();

        /// <summary>
        /// Remaining uncertainties and recommended follow-up research areas.
        /// Useful if the user wants "phase 2" deeper research.
        /// </summary>
        public List<string> FollowUpIdeas { get; init; } = new();
    }
}
