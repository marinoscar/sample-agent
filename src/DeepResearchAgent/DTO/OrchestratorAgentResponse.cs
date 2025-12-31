using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.DTO
{
    public sealed class OrchestratorAgentResponse
    {
        /// <summary>
        /// The current phase of the workflow.
        /// Used by the caller/UI to show progress and to resume runs deterministically.
        /// Example values: "intake", "plan", "retrieve", "synthesize", "final".
        /// </summary>
        public required string Phase { get; init; }

        /// <summary>
        /// True when the system must ask the user follow-up questions before proceeding.
        /// When true, populate <see cref="ClarifyingQuestions"/> and pause orchestration.
        /// </summary>
        public bool NeedsClarification { get; init; }

        /// <summary>
        /// The clarifying questions to ask the user, if <see cref="NeedsClarification"/> is true.
        /// Keep this list short (0–3 questions) to reduce friction.
        /// </summary>
        public List<string> ClarifyingQuestions { get; init; } = new();

        /// <summary>
        /// Defaults/assumptions the system will apply if the user does not answer clarifying questions.
        /// This improves usability and makes runs reproducible.
        /// Example keys: "audience", "timeframe", "scope", "output_format".
        /// </summary>
        public Dictionary<string, string> Assumptions { get; init; } = new();

        /// <summary>
        /// The final research plan selected for execution.
        /// Present when Phase is "plan" or later.
        /// </summary>
        public ResearchPlanDto? Plan { get; init; }

        /// <summary>
        /// High-level status messages that can be shown to users/logs.
        /// Example: "Dispatched 8 sub-questions to WebResearchAgent."
        /// </summary>
        public List<string> Status { get; init; } = new();

        /// <summary>
        /// If true, the orchestrator believes enough evidence exists to proceed to synthesis.
        /// This is where your "quality gates" are enforced (min sources, recency, etc.).
        /// </summary>
        public bool ReadyForSynthesis { get; init; }

        /// <summary>
        /// Optional list of sub-question IDs that require more research (e.g., not enough sources).
        /// Used to trigger targeted re-search loops.
        /// </summary>
        public List<string> SubQuestionsNeedingMoreResearch { get; init; } = new();

        /// <summary>
        /// The assembled draft report returned from the SynthesizerAgent.
        /// Present in Phase "synthesize" or "final".
        /// </summary>
        public DraftReportDto? DraftReport { get; init; }
    }
}
