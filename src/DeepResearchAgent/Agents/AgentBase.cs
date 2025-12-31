using AgentFramework.Core.Agents;
using AgentFramework.Core.Configuration;
using Microsoft.Agents.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepResearchAgent.Agents
{
    public abstract class AgentBase
    {

        private readonly AgentFactory _agentFactory;

        protected AgentBase(AgentFactory agentFactory)
        {
            _agentFactory = agentFactory ?? throw new ArgumentNullException(nameof(agentFactory));
        }

        protected string GetProvider()
        {
            return "OpenAI";
        }

        protected string GetModel()
        {
            return "gpt-4o";
        }

        protected abstract string GetInstructions();

        protected abstract AgentConfiguration GetConfiguration();

        public virtual AIAgent CreateAgent()
        {
            return _agentFactory.CreateAgent(GetConfiguration());
        }

    }
}
