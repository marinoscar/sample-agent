using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.DTO
{
    public sealed class ClarifierAgentResponse
    {
        /// <summary>
        /// True if the system must ask the user follow-up questions before producing the ResearchBrief.
        /// When true, populate Questions and AssumptionsIfNoAnswer. Brief should be null.
        /// </summary>
        public bool NeedsClarification { get; init; }

        /// <summary>
        /// 0–3 questions max. The user-facing questions to remove ambiguity.
        /// </summary>
        public List<string> Questions { get; init; } = new();

        /// <summary>
        /// Defaults to apply if the user does not answer.
        /// Keys: "audience", "scope", "timeframe", "output_format", etc.
        /// </summary>
        public Dictionary<string, string> AssumptionsIfNoAnswer { get; init; } = new();

        /// <summary>
        /// The fully formed ResearchBrief (Scope artifact).
        /// Only set when NeedsClarification is false.
        /// </summary>
        public ResearchBrief? Brief { get; init; }
    }
}
