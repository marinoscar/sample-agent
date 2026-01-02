using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.DTO
{
    /// <summary>
    /// The "north star" artifact created during Scope.
    /// This replaces the separate PlannerAgent in the new architecture.
    /// </summary>
    public sealed class ResearchBrief
    {
        /// <summary>
        /// A short, unambiguous restatement of what the research is trying to accomplish.
        /// This is the primary objective used to decide completeness.
        /// </summary>
        public required string Objective { get; init; } = default!;

        /// <summary>
        /// Intended audience for tone, jargon level, and structure.
        /// Example: "executive", "engineers", "mixed".
        /// </summary>
        public required string Audience { get; init; } = default!;

        /// <summary>
        /// The scope boundaries (geography, industry, inclusion/exclusion).
        /// Example: "Upstream oil & gas maintenance, global, exclude consumer examples."
        /// </summary>
        public required string Scope { get; init; } = default!;

        /// <summary>
        /// Time window/recency requirement.
        /// Example: "Prefer 2023–present; include foundational context pre-2023 if needed."
        /// </summary>
        public required string Timeframe { get; init; } = default!;

        /// <summary>
        /// The deliverable format specification.
        /// Example: "1–2 page brief + evidence table + recommendations".
        /// </summary>
        public required string OutputFormat { get; init; } = default!;

        /// <summary>
        /// What "good" looks like: success criteria for the research.
        /// Example: "At least 5 credible sources; 2 primary; include 3 real-world examples; cover risks."
        /// </summary>
        public required string SuccessCriteria { get; init; } = default!;

        /// <summary>
        /// The major subtopics / research threads to cover.
        /// These are used to dispatch WebResearchAgent tasks.
        /// Typical count: 5–12.
        /// </summary>
        public List<ResearchThread> Threads { get; init; } = new();

        /// <summary>
        /// Stopping conditions / quality gates used by the Orchestrator to decide when to stop searching.
        /// </summary>
        public StopConditions StopConditions { get; init; } = new();
    }
}
