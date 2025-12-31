using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.DTO
{
    /// <summary>
    /// A single research thread/subtopic derived from the ResearchBrief.
    /// This is the unit of work for WebResearchAgent.
    /// </summary>
    public sealed class ResearchThread
    {
        /// <summary>
        /// Unique identifier for routing and aggregation (e.g., "T1").
        /// </summary>
        public required string Id { get; init; }

        /// <summary>
        /// The thread prompt/question to research.
        /// Keep it focused and answerable with sources.
        /// </summary>
        public required string Prompt { get; init; }

        /// <summary>
        /// Priority for scheduling and early completion.
        /// Example: "high", "medium", "low".
        /// </summary>
        public string Priority { get; init; } = "medium";

        /// <summary>
        /// Suggested search queries for the WebResearchAgent to try first.
        /// </summary>
        public List<string> SuggestedQueries { get; init; } = new();

        /// <summary>
        /// Output hint to guide how findings should be shaped.
        /// Examples: "definitions", "examples", "pros_cons", "timeline", "metrics".
        /// </summary>
        public string DeliverableHint { get; init; } = "bullet_findings";
    }

}
