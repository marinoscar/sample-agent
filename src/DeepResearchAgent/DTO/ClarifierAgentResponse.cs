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
        /// Indicates whether the user must answer follow-up questions before planning/research begins.
        /// </summary>
        public bool NeedsClarification { get; init; }

        /// <summary>
        /// Clarifying questions to ask (recommended max 3).
        /// When <see cref="NeedsClarification"/> is false, this should be empty.
        /// </summary>
        public List<string> Questions { get; init; } = new();

        /// <summary>
        /// Assumptions to apply if the user does not provide answers.
        /// Keys should be stable and predictable (e.g., "audience", "timeframe", "scope", "output_format").
        /// </summary>
        public Dictionary<string, string> AssumptionsIfNoAnswer { get; init; } = new();

        /// <summary>
        /// Normalized request derived from user input + assumptions.
        /// This becomes the input to the PlannerAgent.
        /// </summary>
        public ResearchTopicRequest? NormalizedRequest { get; init; }
    }
}
