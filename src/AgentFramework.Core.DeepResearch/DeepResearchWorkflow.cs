using AgentFramework.Core.Agents;
using AgentFramework.Core.DeepResearch.Steps;
using AgentFramework.Core.Workflows;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.DependencyInjection;
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

        private AIAgent _startAgent;

        public DeepResearchWorkflow(IServiceProvider services) : 
            this(services.GetRequiredService<AgentFactory>(), services.GetRequiredService<ILoggerFactory>())
        {
            
        }

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

            _startAgent = scoping;

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

        /// <inheritdoc/>
        public override AIAgent GetStartAgent()
        {
            if (_startAgent == null) throw new InvalidOperationException("You need to run the Build method first");
            return _startAgent;
        }
    }
}
