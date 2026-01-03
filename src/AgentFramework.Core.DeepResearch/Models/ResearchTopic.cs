using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.DeepResearch.Models
{
    [Description("User-scoped research topic and constraints. Produced by ScopingAgent and used to drive planning and execution.")]
    public class ResearchTopic
    {
        [Description("The research topic or question (free text) provided by the user.")]
        public required string Topic { get; init; }

        [Description("Indicates whether the topic needs clarification from the user before proceeding.")]
        public bool NeedsClarification { get; init; } = false;

        [Description("Clarifying questions to ask the user if NeedsClarification is true.")]
        public List<string> ClarificationQuestions { get; init; } = new();

        [Description("Target audience hint (e.g., 'exec', 'technical', 'mixed'). If null, the system will infer defaults.")]
        public string? Audience { get; init; }

        [Description("Desired output format hint (e.g., 'brief', 'deep_dive', 'comparison'). If null, the system will infer defaults.")]
        public string? OutputFormat { get; init; }

        [Description("Scope constraints (e.g., geography, timeframe, domain boundaries, allowed/disallowed sources).")]
        public string? Constraints { get; init; }

        [Description("A concise but specific definition of what the research will cover and what 'done' looks like.")]
        public string? DetailedResearchScope { get; init; }
    }
}
