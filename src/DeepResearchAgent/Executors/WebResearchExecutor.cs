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
    /// Research executor: calls WebResearchAgent for the current thread and returns structured findings.
    /// </summary>
    public sealed class WebResearchExecutor : Executor<ThreadWorkItem, WebResearchAgentResponse>
    {
        public const string ResearchScope = "ResearchState";

        private readonly AIAgent _webResearchAgent;

        public WebResearchExecutor(AIAgent webResearchAgent) : base(nameof(WebResearchExecutor))
        {
            _webResearchAgent = webResearchAgent ?? throw new ArgumentNullException(nameof(webResearchAgent));
        }

        public override async ValueTask<WebResearchAgentResponse> HandleAsync(
            ThreadWorkItem message,
            IWorkflowContext context,
            CancellationToken cancellationToken = default)
        {
            if (message.IsDone)
                throw new InvalidOperationException("WebResearchExecutor cannot run when ThreadWorkItem is done.");

            var brief = await context.ReadStateAsync<ResearchBrief>("brief", scopeName: ResearchScope)
                       ?? throw new InvalidOperationException("ResearchBrief not found in workflow state.");

            var thread = brief.Threads.FirstOrDefault(t => t.Id == message.ThreadId)
                         ?? throw new InvalidOperationException($"Thread '{message.ThreadId}' not found in ResearchBrief.");

            var input = new WebResearchAgentInput
            {
                Brief = brief,
                Thread = thread
            };

            var response = await _webResearchAgent.RunAsync(
                JsonSerializer.Serialize(input),
                cancellationToken: cancellationToken);

            return JsonSerializer.Deserialize<WebResearchAgentResponse>(response.Text)
                   ?? throw new InvalidOperationException("WebResearchAgent returned invalid JSON.");
        }
    }
}
