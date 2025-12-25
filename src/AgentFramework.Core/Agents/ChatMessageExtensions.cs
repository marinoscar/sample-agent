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

        public static AgentMessage ToAgentMessage(this ChatMessage chatMessage, AgentChatMetadata agentInfo)
        {
            if (chatMessage is null) throw new ArgumentNullException(nameof(chatMessage));
            if (agentInfo is null) throw new ArgumentNullException(nameof(agentInfo));

            return new AgentMessage
            {
                ThreadId = agentInfo.ThreadId,
                AgentId = agentInfo.AgentId,
                AgentName = agentInfo.AgentName,
                MessageText = chatMessage.Text ?? string.Empty,
                SerializedMessage = JsonSerializer.Serialize(chatMessage, JsonOptions),
                UtcCreatedAt = chatMessage.CreatedAt.HasValue ? chatMessage.CreatedAt.Value.UtcDateTime : DateTime.UtcNow
            };
        }

        public static ChatMessage ToChatMessage(this AgentMessage agentMessage)
        {
            if (agentMessage is null)
                throw new ArgumentNullException(nameof(agentMessage));

            if (string.IsNullOrWhiteSpace(agentMessage.SerializedMessage))
                throw new InvalidOperationException("SerializedMessage is empty.");

            var chatMessage = JsonSerializer.Deserialize<ChatMessage>(
                agentMessage.SerializedMessage,
                JsonOptions);

            if (chatMessage is null)
                throw new InvalidOperationException("Failed to deserialize ChatMessage.");

            return chatMessage;
        }
    }
}
