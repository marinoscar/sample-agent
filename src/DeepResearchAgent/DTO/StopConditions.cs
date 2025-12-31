using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.DTO
{
    /// <summary>
    /// Stopping conditions and quality gates for a research run.
    /// Used by the Orchestrator to decide when to stop searching and move to synthesis.
    /// </summary>
    public sealed class StopConditions
    {
        /// <summary>
        /// Minimum sources per sub-question before considering it "complete".
        /// </summary>
        public int MinSourcesPerSubQuestion { get; init; } = 3;

        /// <summary>
        /// Desired number of primary sources across the full report.
        /// Primary sources are official docs, regulators, standards bodies, filings, academic papers, etc.
        /// </summary>
        public int MinPrimarySourcesTotal { get; init; } = 2;

        /// <summary>
        /// When recency matters, prefer sources published within this window (in days).
        /// Example: 365 for last year.
        /// </summary>
        public int RecencyPreferenceDays { get; init; } = 730;

        /// <summary>
        /// Instruction for handling conflicting sources.
        /// Example: "Capture both positions and label uncertainty."
        /// </summary>
        public string ConflictHandling { get; init; } =
            "If sources disagree, capture both positions and label uncertainty.";
    }
}
