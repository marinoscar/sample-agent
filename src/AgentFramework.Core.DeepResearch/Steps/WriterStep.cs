using AgentFramework.Core.DeepResearch.Models;
using AgentFramework.Core.Workflows;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.DeepResearch.Steps
{
    public class WriterStep : AgentStepBase<ResearchAggregate, FinalResearchReport>
    {
        public WriterStep(AIAgent agent, ILoggerFactory loggerFactory, AgentThread thread = null, ExecutorOptions? options = null, bool declareCrossRunShareable = false) : base(nameof(WriterStep), agent, loggerFactory, thread, options, declareCrossRunShareable)
        {
        }

        public override async ValueTask<FinalResearchReport> RunStepAsync(ResearchAggregate message, IWorkflowContext context, CancellationToken cancellationToken = default)
        {
            var prompt = Serialize(message);
            var response = await Agent.RunAsync(prompt, cancellationToken: cancellationToken);
            var result = Deserialize<FinalResearchReport>(response.Text);
            return result;
        }
    }
}
