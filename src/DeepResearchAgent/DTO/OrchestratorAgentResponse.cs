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
        /// Workflow phase marker for progress and resumability.
        /// Expected: "scope", "research", "write", "final".
        /// </summary>
        public required string Phase { get; init; }

        /// <summary>
        /// True when the system must ask the user questions before continuing.
        /// </summary>
        public bool NeedsClarification { get; init; }

        /// <summary>
        /// Questions to ask the user if NeedsClarification is true.
        /// </summary>
        public List<string> ClarifyingQuestions { get; init; } = new();

        /// <summary>
        /// Defaults used when user doesn’t answer clarifying questions.
        /// </summary>
        public Dictionary<string, string> Assumptions { get; init; } = new();

        /// <summary>
        /// The ResearchBrief produced during Scope.
        /// Present from Phase "research" onward.
        /// </summary>
        public ResearchBrief? Brief { get; init; }

        /// <summary>
        /// Aggregated findings per thread from WebResearchAgent.
        /// Present from Phase "write" onward (or earlier if streaming progress).
        /// </summary>
        public List<WebResearchAgentResponse> ThreadFindings { get; init; } = new();

        /// <summary>
        /// IDs of threads needing more research due to failing quality gates (too few sources, low confidence, etc.).
        /// </summary>
        public List<string> ThreadsNeedingMoreResearch { get; init; } = new();

        /// <summary>
        /// Indicates whether the Orchestrator believes the system has enough evidence to proceed to writing.
        /// </summary>
        public bool ReadyForWrite { get; init; }

        /// <summary>
        /// Draft report returned by Synthesizer.
        /// Present in Phase "write" or "final".
        /// </summary>
        public DraftReport? DraftReport { get; init; }

        /// <summary>
        /// Optional status messages for UI/logging.
        /// Example: "Completed 6/8 research threads; re-running T3 for higher-quality sources."
        /// </summary>
        public List<string> Status { get; init; } = new();
    }
}
