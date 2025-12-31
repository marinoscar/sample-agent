using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.DTO
{
    public sealed class WebResearchAgentResponse
    {
        /// <summary>
        /// The thread identifier that these findings correspond to.
        /// This is how the Orchestrator aggregates thread results.
        /// </summary>
        public required string ThreadId { get; init; }

        /// <summary>
        /// Claims that answer the thread prompt, each with citations.
        /// </summary>
        public List<ClaimFinding> Findings { get; init; } = new();

        /// <summary>
        /// Gaps that the agent could not fill with confidence.
        /// The Orchestrator can decide to iterate with refined searches.
        /// </summary>
        public List<string> OpenGaps { get; init; } = new();

        /// <summary>
        /// Lightweight audit trail of queries attempted.
        /// </summary>
        public List<SearchLogEntry> SearchLog { get; init; } = new();

        /// <summary>
        /// Count of sources classified as primary in this thread response.
        /// Helpful for Orchestrator quality gating.
        /// </summary>
        public int PrimarySourceCount { get; init; }

        /// <summary>
        /// Total distinct sources referenced across all findings in this thread response.
        /// Helpful for Orchestrator quality gating.
        /// </summary>
        public int TotalSourceCount { get; init; }
    }

}
