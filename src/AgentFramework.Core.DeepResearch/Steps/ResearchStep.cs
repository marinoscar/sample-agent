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
    public class ResearchStep : AgentStepBase<ResearchPlan, ResearchAggregate>
    {
        public ResearchStep(AIAgent agent, ILoggerFactory loggerFactory, AgentThread thread = null, ExecutorOptions? options = null, bool declareCrossRunShareable = false) : base(nameof(ResearchStep), agent, loggerFactory, thread, options, declareCrossRunShareable)
        {
        }

        public override async ValueTask<ResearchAggregate> RunStepAsync(ResearchPlan message, IWorkflowContext context, CancellationToken cancellationToken = default)
        {
            var result = new ResearchAggregate
            {
                Topic = message.Topic,
                Audience = message.Audience,
                OutputFormat = message.OutputFormat,
                Constraints = message.Constraints,
                PlannerNotes = message.PlannerNotes,
                ResearchTopicList = message.ResearchTopicList,
                ItemFindingsMarkdown = new List<string>(),
                ItemSources = new List<string>()
            };
            foreach (var researchItem in message.ResearchTopicList)
            {
                var task = new ResearchTask
                {
                    Topic = message.Topic, Audience = message.Audience, Constraints = message.Constraints,
                    ResearchItem = researchItem, PlannerNotes = message.PlannerNotes
                };

                var jsonInput = Serialize(researchItem);
                var response = await Agent.RunAsync(jsonInput, cancellationToken: cancellationToken);
                var aggregate = Deserialize<ResearchAggregate>(response.Text);

                result.ItemFindingsMarkdown.AddRange(aggregate.ItemFindingsMarkdown);
                result.ItemSources.AddRange(aggregate.ItemSources);
            }

            return result;
        }
    }
}
