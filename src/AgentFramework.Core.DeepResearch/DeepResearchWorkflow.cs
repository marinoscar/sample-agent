using AgentFramework.Core.Agents;
using AgentFramework.Core.DeepResearch.Steps;
using AgentFramework.Core.Workflows;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.DeepResearch
{
    public class DeepResearchWorkflow : AgentWorkflowBase
    {
        public DeepResearchWorkflow(AgentFactory agentFactory, ILoggerFactory loggerFactory) : base("DeepResearchAgent", "Deep Research Agent", agentFactory, loggerFactory)
        {
        }

        public override Workflow Build()
        {
            var cb = new AgentBuilder();
            var scoping = Factory.CreateAgent(cb.CreateScopingAgentConfig());
            var planning = Factory.CreateAgent(cb.CreatePlanningAgentConfig());
            var research = Factory.CreateAgent(cb.CreateResearchAgentConfig());
            var writer = Factory.CreateAgent(cb.CreateWriterAgentConfig());

            var mainThread = scoping.GetNewThread();

            var scopingStep = new ScopingStep(Factory.CreateAgent(cb.CreateScopingAgentConfig()), LoggerFactory);
            var planningStep = new PlanningStep(planning, LoggerFactory);
            var researchStep = new ResearchStep(research, LoggerFactory);
            var writingStep = new WriterStep(writer, LoggerFactory);

            var workflow = new WorkflowBuilder(scopingStep)
                .AddEdge(planningStep, researchStep)
                .AddEdge(researchStep, writingStep)
                .WithOutputFrom(writingStep)
                .Build();

            return workflow;
        }
    }
}
