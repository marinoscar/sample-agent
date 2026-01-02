using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.Models
{
    /// <summary>
    /// Represents a research topic or question, including user-provided context, clarification needs, and optional
    /// guidance for audience, output format, and scope.
    /// </summary>
    /// <remarks>Use this class to encapsulate all relevant information required to define and refine a
    /// research request. Properties such as audience, output format, and constraints are optional and, if not
    /// specified, will be inferred by the system. Clarification questions can be provided to gather additional details
    /// from the user when the topic is ambiguous or incomplete.</remarks>
    [Description("Represents a research topic or question, including user-provided context, clarification needs, and optional guidance for audience, output format, and scope.")]
    public class ResearchTopic
    {
        /// <summary>
        /// The research topic or question (free text) provided by the user.
        /// </summary>
        public required string Topic { get; init; }

        /// <summary>
        /// Identifies if the topic needs clarification from the user before proceding
        /// </summary>
        public bool NeedsClarification { get; init; } = false;

        /// <summary>
        /// The list of questions to ask the user for clarification
        /// </summary>
        public List<string> ClarificationQuestions { get; init; } = new();

        /// <summary>
        /// Optional audience hint (e.g., "exec", "technical", "mixed").
        /// If null, the system will infer defaults.
        /// </summary>
        public string? Audience { get; init; }

        /// <summary>
        /// Optional desired output format hint (e.g., "brief", "deep_dive", "comparison").
        /// If null, the system will infer defaults.
        /// </summary>
        public string? OutputFormat { get; init; }


        /// <summary>
        /// Optional scope constraints (e.g., "US only", "upstream O&G", "healthcare", "last 2 years", "timeframe").
        /// If null, the system will infer defaults.
        /// </summary>
        public string? Constraints { get; init; }

        /// <summary>
        /// The detail scope for the research to be performed for the user
        /// </summary>
        public string? DetailedResearchScope { get; init; }
    }
}
