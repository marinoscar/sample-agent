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
    /// Scope executor: calls ClarifierAgent to produce either (a) clarifying questions OR (b) a ResearchBrief.
    /// </summary>
    public sealed class ScopeExecutor : Executor<ResearchTopicRequest, ClarifierAgentResponse>
    {
        private readonly AIAgent _clarifierAgent;

        public ScopeExecutor(AIAgent clarifierAgent) : base(nameof(ScopeExecutor))
        {
            _clarifierAgent = clarifierAgent ?? throw new ArgumentNullException(nameof(clarifierAgent));
        }

        public override async ValueTask<ClarifierAgentResponse> HandleAsync(
            ResearchTopicRequest message,
            IWorkflowContext context,
            CancellationToken cancellationToken = default)
        {
            var response = await _clarifierAgent.RunAsync(
                JsonSerializer.Serialize(message),
                cancellationToken: cancellationToken);


            return JsonSerializer.Deserialize<ClarifierAgentResponse>(response.Text)
                   ?? throw new InvalidOperationException("ClarifierAgent returned invalid JSON.");
        }
    }
}
