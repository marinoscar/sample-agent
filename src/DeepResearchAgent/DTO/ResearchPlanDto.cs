using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.DTO
{
    public sealed class ResearchPlanDto
    {
        /// <summary>
        /// The top-level topic for traceability.
        /// </summary>
        public required string Topic { get; init; }

        /// <summary>
        /// A short statement describing what decision/question the research aims to support.
        /// </summary>
        public required string Objective { get; init; }

        /// <summary>
        /// Sub-questions that the WebResearchAgent should answer.
        /// </summary>
        public List<SubQuestion> SubQuestions { get; init; } = new();

        /// <summary>
        /// Stop conditions / quality gates.
        /// The Orchestrator uses this to decide when to stop searching and move to synthesis.
        /// </summary>
        public StopConditions StopConditions { get; init; } = new();
    }
}
