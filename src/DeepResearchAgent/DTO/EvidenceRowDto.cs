using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.DTO
{
    public sealed class EvidenceRowDto
    {
        /// <summary>
        /// The claim being supported.
        /// </summary>
        public required string Claim { get; init; }

        /// <summary>
        /// A small list of "best" sources (URLs) supporting the claim.
        /// </summary>
        public List<string> BestSources { get; init; } = new();
    }
}
