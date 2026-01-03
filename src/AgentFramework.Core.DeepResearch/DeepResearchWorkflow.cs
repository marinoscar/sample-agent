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
            throw new NotImplementedException();
        }
    }
}
