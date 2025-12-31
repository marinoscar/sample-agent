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
        /// The sub-question identifier that these findings answer (e.g., "SQ3").
        /// This is how the Orchestrator merges results.
        /// </summary>
        public required string SubQuestionId { get; init; }

        /// <summary>
        /// The claims and their supporting sources discovered during web research.
        /// </summary>
        public List<ClaimFinding> Findings { get; init; } = new();

        /// <summary>
        /// Any gaps the agent could not confidently answer with available sources.
        /// The Orchestrator can decide to re-search with stricter queries.
        /// </summary>
        public List<string> OpenGaps { get; init; } = new();

        /// <summary>
        /// A lightweight audit trail of search queries attempted and what happened.
        /// Helps you debug why a sub-question returned weak results.
        /// </summary>
        public List<SearchLogEntry> SearchLog { get; init; } = new();
    }

}
