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
        /// The drafted report ready for Orchestrator finalization.
        /// </summary>
        public required DraftReport DraftReport { get; init; }

        /// <summary>
        /// Claims excluded due to weak/insufficient evidence.
        /// Helps keep the final output honest and auditable.
        /// </summary>
        public List<string> ExcludedClaims { get; init; } = new();

        /// <summary>
        /// Optional follow-up research ideas if the user wants deeper iteration.
        /// </summary>
        public List<string> FollowUpIdeas { get; init; } = new();
    }
}
