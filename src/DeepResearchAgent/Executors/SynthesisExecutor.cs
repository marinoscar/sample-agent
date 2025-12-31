using DeepResearchAgent.DTO;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeepResearchAgent.Executors
{
    /// <summary>
    /// Write executor: when research is complete (ThreadWorkItem.Done), call SynthesizerAgent to produce final report.
    /// </summary>
    public sealed class SynthesisExecutor : Executor<ThreadWorkItem, SynthesizerAgentResponse>
    {
        public const string ResearchScope = "ResearchState";
        public const string FindingsScope = "FindingsState";

        private readonly AIAgent _synthesizerAgent;

        public SynthesisExecutor(AIAgent synthesizerAgent) : base(nameof(SynthesisExecutor))
        {
            _synthesizerAgent = synthesizerAgent ?? throw new ArgumentNullException(nameof(synthesizerAgent));
        }

        public override async ValueTask<SynthesizerAgentResponse> HandleAsync(
            ThreadWorkItem message,
            IWorkflowContext context,
            CancellationToken cancellationToken = default)
        {
            if (!message.IsDone)
                throw new InvalidOperationException("SynthesisExecutor must only run when research queue is complete.");

            var brief = await context.ReadStateAsync<ResearchBrief>("brief", scopeName: ResearchScope)
                       ?? throw new InvalidOperationException("ResearchBrief not found in workflow state.");

            var allFindings = new List<WebResearchAgentResponse>();
            foreach (var thread in brief.Threads)
            {
                var f = await context.ReadStateAsync<WebResearchAgentResponse>(thread.Id, scopeName: FindingsScope);
                if (f is not null) allFindings.Add(f);
            }

            var input = new SynthesizerAgentInput
            {
                Brief = brief,
                ThreadFindings = allFindings,
                AsOf = DateTime.UtcNow.ToString("yyyy-MM-dd")
            };

            var response = await _synthesizerAgent.RunAsync(
                JsonSerializer.Serialize(input),
                cancellationToken: cancellationToken);

            return JsonSerializer.Deserialize<SynthesizerAgentResponse>(response.Text)
                   ?? throw new InvalidOperationException("SynthesizerAgent returned invalid JSON.");
        }
    }
}
