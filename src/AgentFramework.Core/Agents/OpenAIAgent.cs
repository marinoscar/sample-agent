using AgentFramework.Core.Configuration;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Agents
{
    public class OpenAIAgent
    {
        private AIAgent _agent;

        public OpenAIAgent(AgentSettings settings)
        {
            var webSearch = new WebSearchTool();
            var aiTool = webSearch.AsAITool();
            var tools = new List<AITool> { aiTool };

            var client = new OpenAIClient(settings.ApiKey);

            var responsesClient = client.GetResponsesClient(settings.Model);
            _agent = responsesClient.CreateAIAgent(settings.Instructions, tools: tools);

        }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;



    }
}
