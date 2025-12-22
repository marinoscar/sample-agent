using AgentFramework.Core.Configuration;
using Microsoft.Agents.AI;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Agents
{
    public abstract class BaseAgentWrapper
    {

        protected BaseAgentWrapper(AgentSettings settings)
        {
            InitializeAgent(settings);
        }



        public AgentThread AgentThread { get; set; } = default!;
        public AIAgent Agent { get; protected set; } = default!;

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public void StartThread()
        {
            if (AgentThread == null)
                AgentThread = Agent.GetNewThread();
        }

        protected abstract void InitializeAgent(AgentSettings settings);





    }
}
