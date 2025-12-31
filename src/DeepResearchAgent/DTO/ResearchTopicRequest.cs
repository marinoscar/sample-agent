using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.DTO
{
    /// <summary>
    /// Entry request for a DeepResearch run.
    /// Provided by user or upstream application layer.
    /// </summary>
    public sealed class ResearchTopicRequest
    {
        /// <summary>
        /// The research topic or question (free text) provided by the user.
        /// </summary>
        public required string Topic { get; init; }

        /// <summary>
        /// Optional audience hint (e.g., "exec", "technical", "mixed").
        /// If null, the system will infer defaults.
        /// </summary>
        public string? Audience { get; init; }

        /// <summary>
        /// Optional desired output format hint (e.g., "brief", "deep_dive", "comparison").
        /// If null, the system will infer defaults.
        /// </summary>
        public string? OutputFormat { get; init; }

        /// <summary>
        /// Optional timeframe hint (e.g., "last_12_months", "since_2022", "all_time").
        /// If null, the system will infer defaults.
        /// </summary>
        public string? Timeframe { get; init; }

        /// <summary>
        /// Optional scope constraints (e.g., "US only", "upstream O&G", "healthcare").
        /// If null, the system will infer defaults.
        /// </summary>
        public string? Scope { get; init; }
    }
}
