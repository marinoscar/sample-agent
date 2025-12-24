using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AgentFramework.Core.Data
{
    public class SqlChatMessageStore : ChatMessageStore
    {

        private readonly IAgentMessageStore _agentMessageStore;

        public SqlChatMessageStore(IAgentMessageStore agentMessageStore)
        {
            _agentMessageStore = agentMessageStore ?? throw new ArgumentNullException(nameof(agentMessageStore));
        }

        public override Task AddMessagesAsync(IEnumerable<ChatMessage> messages, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<ChatMessage>> GetMessagesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override JsonElement Serialize(JsonSerializerOptions? jsonSerializerOptions = null)
        {
            throw new NotImplementedException();
        }
    }
}
