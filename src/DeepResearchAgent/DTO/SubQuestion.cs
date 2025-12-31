using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.DTO
{
    /// <summary>
    /// A single sub-question in a research plan.
    /// The Planner emits these; the Orchestrator routes them to the WebResearchAgent.
    /// </summary>
    public sealed class SubQuestion
    {
        /// <summary>
        /// Unique identifier used for routing and aggregation (e.g., "SQ1").
        /// </summary>
        public required string Id { get; init; }

        /// <summary>
        /// The actual research sub-question to answer.
        /// </summary>
        public required string Question { get; init; }

        /// <summary>
        /// Relative importance for prioritization (e.g., "high", "medium", "low").
        /// The Orchestrator may skip low priority if time/resource constrained.
        /// </summary>
        public string Priority { get; init; } = "medium";

        /// <summary>
        /// Suggested search queries that the WebResearchAgent should try first.
        /// </summary>
        public List<string> SuggestedQueries { get; init; } = new();

        /// <summary>
        /// What the Planner expects back (e.g., "bullet_findings", "pros_cons", "timeline", "definitions").
        /// Helps keep the WebResearchAgent focused.
        /// </summary>
        public string DeliverableHint { get; init; } = "bullet_findings";
    }
}
