using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.DTO
{
    /// <summary>
    /// A single claim supported by one or more sources.
    /// WebResearchAgent produces these; Synthesizer consumes them.
    /// </summary>
    public sealed class ClaimFinding
    {
        /// <summary>
        /// The claim statement in plain English.
        /// Should be specific enough to be meaningful and verifiable.
        /// </summary>
        public required string Claim { get; init; }

        /// <summary>
        /// Confidence score from 0.0 to 1.0.
        /// Use 0.9+ only when multiple strong sources corroborate.
        /// </summary>
        public double Confidence { get; init; }

        /// <summary>
        /// Sources that support the claim (at least 1).
        /// The Orchestrator should reject claims with an empty list.
        /// </summary>
        public List<SourceRecord> SupportingSources { get; init; } = new();
    }

}
