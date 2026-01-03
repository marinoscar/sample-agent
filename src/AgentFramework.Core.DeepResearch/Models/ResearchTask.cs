using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.DeepResearch.Models
{
    [Description("Represents a single atomic research task derived from a research plan.")]
    public class ResearchTask
    {
        [Description("The original topic text copied from ResearchTopic for traceability.")]
        public required string Topic { get; init; }


        [Description("Audience copied from ResearchTopic (or inferred).")]
        public string? Audience { get; init; }


        [Description("Constraints copied from ResearchTopic (or inferred).")]
        public string? Constraints { get; init; }


        [Description("Planning assumptions, inferred constraints, and guidance such as preferred source types.")]
        public string? PlannerNotes { get; init; }

        [Description("Single research item to investigate. This is a concise, atomic research objective.")]
        public string? ResearchItem { get; init; }

    }
}
