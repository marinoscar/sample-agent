using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.DTO
{
    /// <summary>
    /// A normalized citation/source record returned by the WebResearchAgent.
    /// This is intentionally lightweight so you can store it easily and avoid large quote blobs.
    /// </summary>
    public sealed class SourceRecord
    {
        /// <summary>
        /// The document/page title.
        /// </summary>
        public required string Title { get; init; }

        /// <summary>
        /// Publisher or authoring organization (e.g., "NIST", "Microsoft", "IEEE").
        /// </summary>
        public required string Publisher { get; init; }

        /// <summary>
        /// Published date in ISO format (YYYY-MM-DD) if known.
        /// Keep null if unavailable.
        /// </summary>
        public string? PublishedDate { get; init; }

        /// <summary>
        /// Canonical URL used as the citation target.
        /// </summary>
        public required string Url { get; init; }

        /// <summary>
        /// Source classification: "primary" or "secondary".
        /// Helps Orchestrator enforce quality gates.
        /// </summary>
        public string SourceType { get; init; } = "secondary";

        /// <summary>
        /// Short paraphrase or excerpt (avoid long quotes).
        /// This is used to justify a claim and help the Synthesizer write accurately.
        /// </summary>
        public string? Excerpt { get; init; }
    }
}
