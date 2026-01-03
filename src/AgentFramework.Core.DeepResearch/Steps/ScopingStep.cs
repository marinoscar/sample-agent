using AgentFramework.Core.DeepResearch.Models;
using AgentFramework.Core.Workflows;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.DeepResearch.Steps
{
    public class ScopingStep : AgentStepBase<ChatMessage, ResearchTopic>
    {
        public ScopingStep(AIAgent agent, ILoggerFactory loggerFactory, AgentThread thread = null, ExecutorOptions? options = null, bool declareCrossRunShareable = false) : base(nameof(ScopingStep), agent, loggerFactory, thread, options, declareCrossRunShareable)
        {
        }

        public override async ValueTask<ResearchTopic> RunStepAsync(ChatMessage message, IWorkflowContext context, CancellationToken cancellationToken = default)
        {
            var prompt = message.Text;
            var topic = new ResearchTopic
            {
                Topic = prompt
            };
            var jsonInput = Serialize(topic);

            var response = await Agent.RunAsync(jsonInput, cancellationToken: cancellationToken);
            var result = Deserialize<ResearchTopic>(response.Text);
            
            return result;
        }
    }
}
