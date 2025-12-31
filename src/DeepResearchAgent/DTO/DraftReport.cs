using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.DTO
{
    public sealed class DraftReport
    {
        public required string Title { get; init; }
        public required string AsOf { get; init; }
        public List<string> ExecutiveSummary { get; init; } = new();
        public List<FindingWithCitations> KeyFindings { get; init; } = new();
        public List<string> RisksAndUncertainties { get; init; } = new();
        public List<string> Recommendations { get; init; } = new();
        public List<EvidenceRow> EvidenceTable { get; init; } = new();
    }

    public sealed class FindingWithCitations
    {
        public required string Finding { get; init; }
        public List<string> Citations { get; init; } = new();
    }

    public sealed class EvidenceRow
    {
        public required string Claim { get; init; }
        public List<string> BestSources { get; init; } = new();
    }
}
