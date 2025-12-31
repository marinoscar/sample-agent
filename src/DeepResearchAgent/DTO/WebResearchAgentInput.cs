using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.DTO
{
    /// <summary>
    /// Input envelope you pass to WebResearchAgent (recommended).
    /// Keeps agent runs deterministic and prevents missing context.
    /// </summary>
    public sealed class WebResearchAgentInput
    {
        /// <summary>
        /// The thread to research (unit of work).
        /// </summary>
        public required ResearchThread Thread { get; init; }

        /// <summary>
        /// The brief provides global constraints: scope/timeframe/quality expectations.
        /// </summary>
        public required ResearchBrief Brief { get; init; }
    }
}
