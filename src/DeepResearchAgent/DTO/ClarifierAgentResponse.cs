using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DeepResearchAgent.DTO
{
    public sealed class ClarifierAgentResponse
    {
        /// <summary>
        /// True if the system must ask the user follow-up questions before producing the ResearchBrief.
        /// When true, populate Questions and AssumptionsIfNoAnswer. Brief should be null.
        /// </summary>
        public required bool NeedsClarification { get; init; }

        /// <summary>
        /// 0–3 questions max. The user-facing questions to remove ambiguity.
        /// </summary>
        public required List<string> Questions { get; init; } = new();

        /// <summary>
        /// Defaults to apply if the user does not answer.
        /// Keys: "audience", "scope", "timeframe", "output_format", etc.
        /// </summary>
        //public required Dictionary<string, string> AssumptionsIfNoAnswer { get; init; } = new();

        /// <summary>
        /// The fully formed ResearchBrief (Scope artifact).
        /// Only set when NeedsClarification is false.
        /// </summary>
        //public required ResearchBrief Brief { get; init; } = new()
        //{
        //    Audience = string.Empty,
        //    Objective = string.Empty,
        //    OutputFormat = string.Empty,
        //    Scope = string.Empty,
        //    SuccessCriteria = string.Empty,
        //    Timeframe = string.Empty
        //};
    }
}
