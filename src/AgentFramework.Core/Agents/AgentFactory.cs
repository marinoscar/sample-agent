using AgentFramework.Core.Configuration;
using AgentFramework.Core.Data;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Agents
{
    public class AgentFactory
    {

        private readonly ILoggerFactory? _logger;
        private readonly ChatMessageStore? _chatMessageStore;

        public AgentFactory(ChatMessageStore? chatMessageStore, ILoggerFactory? loggerFactory = null)
        {
            _logger = loggerFactory;
            _chatMessageStore = chatMessageStore ?? new SqlChatMessageStore(new AgentMessageStoreService(() => new SqliteAgentMessageContext()));
        }

        public ResponsesClient CreateOpenAIResponsesClient()
        {
            var client = new OpenAIClient("KEY");
            var responses =  client.GetResponsesClient("MODEL");
            return responses;
        }

        public AIAgent CreateOpenAIAgent(AgentConfiguration agentSettings)
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
                    ToolMode = agentSettings.GetToolMode(),
                    Tools = new AgentToolFactory().GetTools(agentSettings.ToolList),
                },
                ChatMessageStoreFactory = (context) =>
                {
                    return _chatMessageStore!;
                }
            }, loggerFactory: _logger);
            return agent;
        }
    }
}
