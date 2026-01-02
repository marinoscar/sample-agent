using DeepResearchAgent.Models;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.Steps
{
    /// <summary>
    /// First step of the workflow
    /// </summary>
    public class IntakeStep : StepBase<ChatMessage>
    {

        private readonly AIAgent _agent;

        public IntakeStep(AIAgent agent, ILoggerFactory loggerFactory, ExecutorOptions? options = null, bool declareCrossRunShareable = false) : base(nameof(IntakeStep), loggerFactory, options, declareCrossRunShareable)
        {
            _agent = agent ?? throw new ArgumentNullException(nameof(agent));
        }

        public override async ValueTask RunStepAsync(ChatMessage message, IWorkflowContext context, CancellationToken cancellationToken = default)
        {
            var response = await _agent.RunAsync([message], cancellationToken: cancellationToken);
            var output = Deserialize<ResearchTopic>(response.Text);
            if (output.NeedsClarification)
                await context.YieldOutputAsync(output);
        }
    }
}
