using AgentFramework.Core.Agents;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AgentFramework.Core.Data
{
    public class SqlChatMessageStore : ChatMessageStore
    {

        private readonly IAgentMessageStore _agentMessageStore;

        public AgentChatMetadata AgentInfo { get; set; } = default!;

        public SqlChatMessageStore(IAgentMessageStore agentMessageStore)
        {
            _agentMessageStore = agentMessageStore ?? throw new ArgumentNullException(nameof(agentMessageStore));
        }

        public override async Task AddMessagesAsync(IEnumerable<ChatMessage> messages, CancellationToken cancellationToken = default)
        {
            await _agentMessageStore.AddRangeAsync(
                AgentInfo,
                messages.Select(m => m.ToAgentMessage(AgentInfo)),
                cancellationToken);
        }

        public override async Task<IEnumerable<ChatMessage>> GetMessagesAsync(CancellationToken cancellationToken = default)
        {
            return await _agentMessageStore
                .GetByThreadIdAsync(AgentInfo, cancellationToken)
                .ContinueWith(t => t.Result.Select(m => m.ToChatMessage()), cancellationToken);
        }

        public override JsonElement Serialize(JsonSerializerOptions? jsonSerializerOptions = null)
        {
            if(jsonSerializerOptions == null)
            {
                jsonSerializerOptions = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                };
            }
            return JsonSerializer.SerializeToElement(AgentInfo, jsonSerializerOptions);
        }
    }
}
