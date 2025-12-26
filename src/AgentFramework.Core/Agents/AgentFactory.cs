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
    public static class AgentFactory
    {

        public static ResponsesClient CreateOpenAIResponsesClient()
        {
            var client = new OpenAIClient("KEY");
            var responses =  client.GetResponsesClient("MODEL");
            return responses;
        }

        public static AIAgent CreateOpenAIAgent(AgentSettings agentSettings)
        {
            var responsesClient = CreateOpenAIResponsesClient();
            var agent = responsesClient.CreateAIAgent(options: new ChatClientAgentOptions()
            {
                Id = agentSettings.Id,
                Name = agentSettings.Name,
                Description = agentSettings.Description,
                ChatOptions = new ChatOptions()
                {
                    Instructions = agentSettings.Instructions,
                    ModelId = agentSettings.Model,
                    Temperature = (float)(agentSettings.Temperature),
                    ResponseFormat = agentSettings.GetResponseFormat(),
                },
                Temperature = agentSettings.Temperature,
                ToolMode = agentSettings.GetToolMode()
            });
            return agent;
        }
    }
}
