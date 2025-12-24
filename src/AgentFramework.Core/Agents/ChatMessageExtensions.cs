using AgentFramework.Core.Data;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AgentFramework.Core.Agents
{
    public static class ChatMessageExtensions
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.Never
        };

        public static AgentMessage ToAgentMessage(this ChatMessage chatMessage, string threadId)
        {
            if (chatMessage is null) throw new ArgumentNullException(nameof(chatMessage));
            if (string.IsNullOrWhiteSpace(threadId)) throw new ArgumentException("ThreadId is required.", nameof(threadId));

            return new AgentMessage
            {
                ThreadId = threadId,
                MessageText = chatMessage.Text ?? string.Empty,
                SerializedMessage = JsonSerializer.Serialize(chatMessage, JsonOptions),
                UtcCreatedAt = chatMessage.CreatedAt.HasValue ? chatMessage.CreatedAt.Value.UtcDateTime : DateTime.UtcNow
            };
        }
    }
}
