using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.DTO
{
    public sealed class DraftReportDto
    {
        /// <summary>
        /// Report title suitable for a document header.
        /// </summary>
        public required string Title { get; init; }

        /// <summary>
        /// "As of" date for the research (e.g., today's date).
        /// Helps readers interpret recency.
        /// </summary>
        public required string AsOf { get; init; }

        /// <summary>
        /// Executive summary bullets intended for quick scanning.
        /// </summary>
        public List<string> ExecutiveSummary { get; init; } = new();

        /// <summary>
        /// Key findings with citations (URLs).
        /// </summary>
        public List<FindingWithCitationsDto> KeyFindings { get; init; } = new();

        /// <summary>
        /// Risks, caveats, and explicit unknowns.
        /// This is critical if sources conflict or the topic is fast-moving.
        /// </summary>
        public List<string> RisksAndUncertainties { get; init; } = new();

        /// <summary>
        /// Recommended actions or conclusions based on the evidence.
        /// </summary>
        public List<string> Recommendations { get; init; } = new();

        /// <summary>
        /// Evidence table mapping claims to best sources.
        /// Useful for auditability and downstream automation.
        /// </summary>
        public List<EvidenceRowDto> EvidenceTable { get; init; } = new();
    }
}
