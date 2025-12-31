using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.DTO
{
    /// <summary>
    /// Input envelope passed to SynthesizerAgent.
    /// </summary>
    public sealed class SynthesizerAgentInput
    {
        /// <summary>
        /// Global research brief created during Scope.
        /// </summary>
        public required ResearchBrief Brief { get; init; }
        /// <summary>
        /// List of findings from each research thread.
        /// </summary>
        public required List<WebResearchAgentResponse> ThreadFindings { get; init; }
        /// <summary>
        /// Gets the date representing the point in time for which the data is relevant.
        /// </summary>
        /// <remarks>The value must be specified in the ISO 8601 format "YYYY-MM-DD". This property is
        /// required and cannot be null or empty.</remarks>
        public required string AsOf { get; init; } // YYYY-MM-DD
    }
}
