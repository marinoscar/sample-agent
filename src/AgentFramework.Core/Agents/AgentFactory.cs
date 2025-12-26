using Microsoft.Agents.AI;
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

        public static AIAgent CreateOpenAIAgent(string instructions)
        {
            var responsesClient = CreateOpenAIResponsesClient();
            var agent = responsesClient.CreateAIAgent(instructions);
            return agent;
        }
    }
}
