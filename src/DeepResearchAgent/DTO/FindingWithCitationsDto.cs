using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.DTO
{
    public sealed class FindingWithCitationsDto
    {
        /// <summary>
        /// A single finding statement.
        /// </summary>
        public required string Finding { get; init; }

        /// <summary>
        /// URLs that support the finding.
        /// The Synthesizer should include at least one citation per major finding.
        /// </summary>
        public List<string> Citations { get; init; } = new();
    }
}
