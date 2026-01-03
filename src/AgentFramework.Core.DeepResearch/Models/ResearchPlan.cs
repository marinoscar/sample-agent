using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.DeepResearch.Models
{
    [Description("Simplified research plan containing a single ordered list of research items. Produced by PlannerAgent.")]
    public class ResearchPlan
    {
        [Description("The original topic text copied from ResearchTopic for traceability.")]
        public required string Topic { get; init; }


        [Description("Audience copied from ResearchTopic (or inferred).")]
        public string? Audience { get; init; }


        [Description("Output format copied from ResearchTopic (or inferred).")]
        public string? OutputFormat { get; init; }


        [Description("Constraints copied from ResearchTopic (or inferred).")]
        public string? Constraints { get; init; }


        [Description("Planning assumptions, inferred constraints, and guidance such as preferred source types.")]
        public string? PlannerNotes { get; init; }


        [Description("Ordered list of research items to investigate. Each entry is a concise, atomic research objective.")]
        public List<string> ResearchTopicList { get; init; } = new();
    }
}
