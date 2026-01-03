using AgentFramework.Core.Agents;
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

            throw new NotImplementedException();
        }
    }
}
