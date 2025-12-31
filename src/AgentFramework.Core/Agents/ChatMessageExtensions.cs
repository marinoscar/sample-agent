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
    /// <summary>
    /// Provides extension methods for converting between <see cref="ChatMessage"/> and <see cref="AgentMessage"/> types.
    /// </summary>
    public static class ChatMessageExtensions
    {
        /// <summary>
        /// JSON serialization options used for converting chat messages to and from serialized format.
        /// Configured with indentation, cycle handling, and to never ignore properties.
        /// </summary>
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.Never
        };

        /// <summary>
        /// Converts a <see cref="ChatMessage"/> to an <see cref="AgentMessage"/> with associated agent metadata.
        /// </summary>
        /// <param name="chatMessage">The chat message to convert.</param>
        /// <param name="agentInfo">The agent metadata containing thread and agent information.</param>
        /// <returns>An <see cref="AgentMessage"/> populated with the chat message content and agent metadata.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="chatMessage"/> or <paramref name="agentInfo"/> is null.</exception>
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

        /// <summary>
        /// Converts an <see cref="AgentMessage"/> back to a <see cref="ChatMessage"/> by deserializing the stored message.
        /// </summary>
        /// <param name="agentMessage">The agent message to convert.</param>
        /// <returns>A <see cref="ChatMessage"/> deserialized from the agent message's serialized content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="agentMessage"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the serialized message is empty or deserialization fails.</exception>
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
