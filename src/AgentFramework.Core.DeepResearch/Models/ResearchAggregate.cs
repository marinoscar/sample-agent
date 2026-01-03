using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.DeepResearch.Models
{
    [Description("Aggregated research results aligned to the plan's ResearchTopicList. Produced by the ResearchExecutor/AggregatorAgent.")]
    public class ResearchAggregate
    {

        [Description("Topic copied from the plan for traceability.")]
        public required string Topic { get; init; }


        [Description("Audience copied from the plan for traceability.")]
        public string? Audience { get; init; }


        [Description("Output format copied from the plan for traceability.")]
        public string? OutputFormat { get; init; }


        [Description("Constraints copied from the plan for traceability.")]
        public string? Constraints { get; init; }


        [Description("Planner notes copied from the plan (assumptions, caveats, and source guidance).")]
        public string? PlannerNotes { get; init; }


        [Description("The research items executed (copied from ResearchPlan.ResearchTopicList) for index alignment.")]
        public List<string> ResearchTopicList { get; init; } = new();

        [Description("Per-item findings aligned by index to ResearchTopicList. Each entry is a compact summary (bullets or short paragraphs).")]
        public List<string> ItemFindingsMarkdown { get; init; } = new();


        [Description("Per-item sources aligned by index. Each entry may contain multiple sources delimited by ' || ' using 'Title | URL'.")]
        public List<string> ItemSources { get; init; } = new();

    }
}
