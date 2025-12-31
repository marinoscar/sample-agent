using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.DTO
{
    /// <summary>
    /// Defines the user-facing request that kicks off a DeepResearch run.
    /// This is what your Orchestrator receives from the user entry point.
    /// </summary>
    public sealed class ResearchTopicRequest
    {
        /// <summary>
        /// The research topic or question (free text) provided by the user.
        /// Example: "Impact of AI agents on Oil & Gas maintenance reliability."
        /// </summary>
        public required string Topic { get; init; }

        /// <summary>
        /// Optional. Describes the intended audience (e.g., "executives", "engineers", "mixed").
        /// When null, the system will infer defaults.
        /// </summary>
        public string? Audience { get; init; }

        /// <summary>
        /// Optional. Desired output format (e.g., "brief", "deep_dive", "comparison", "timeline").
        /// When null, the Orchestrator assumes an exec-friendly brief with an evidence table.
        /// </summary>
        public string? OutputFormat { get; init; }

        /// <summary>
        /// Optional. Time window or recency requirement.
        /// Example: "last_12_months", "since_2022", "all_time".
        /// If null, Orchestrator assumes "last 18–24 months + foundational background".
        /// </summary>
        public string? Timeframe { get; init; }

        /// <summary>
        /// Optional. Scope constraints such as geography/industry.
        /// Example: "US only", "EU regulation", "Oil & Gas upstream".
        /// </summary>
        public string? Scope { get; init; }
    }
}
