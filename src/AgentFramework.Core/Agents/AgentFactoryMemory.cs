using AgentFramework.Core.Configuration;
using Microsoft.Agents.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Agents
{
    public class AgentFactoryMemory
    {
        private static IDictionary<string, AgentConfiguration> _agentConfiguration = new Dictionary<string, AgentConfiguration>();


        internal static void SetConfiguration(string agentId, AgentConfiguration configuration)
        {
            _agentConfiguration[agentId] = configuration;
        }

        public static AgentConfiguration? GetConfiguration(string agentId)
        {
            if (_agentConfiguration.ContainsKey(agentId))
            {
                return _agentConfiguration[agentId];
            }
            return null;
        }
    }
}
