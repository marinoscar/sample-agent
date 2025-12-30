using Microsoft.Agents.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AgentFramework.Core.Data
{
    /// <summary>
    /// Base class for agent-specific chat message stores.
    /// </summary>
    public abstract class AgentChatMessageStoreBase : ChatMessageStore
    {
        /// <summary>
        /// Gets the serialized state of the store.
        /// </summary>
        protected JsonElement SerializedStoreState { get; private set; }

        /// <summary>
        /// Gets the JSON serializer options used for serialization.
        /// </summary>
        protected JsonSerializerOptions JsonSerializerOptions { get; private set; } = default!;

        /// <summary>
        /// Gets the agent metadata information.
        /// </summary>
        public virtual AgentChatMetadata AgentInfo { get; private set; } = default!;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentChatMessageStoreBase"/> class.
        /// </summary>
        /// <param name="agentId">The agent identifier.</param>
        /// <param name="agentName">The agent name.</param>
        /// <param name="serializedStoreState">The serialized store state.</param>
        /// <param name="jsonSerializerOptions">Optional JSON serializer options.</param>
        protected AgentChatMessageStoreBase(string agentId, string agentName, JsonElement serializedStoreState, JsonSerializerOptions? jsonSerializerOptions = null)
        {
            SerializedStoreState = serializedStoreState;
            JsonSerializerOptions = jsonSerializerOptions ?? new JsonSerializerOptions() { 
                WriteIndented = true,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            };
            var threadId = Guid.NewGuid().ToString("N");
            if (serializedStoreState.ValueKind is JsonValueKind.String)
                threadId = serializedStoreState.Deserialize<string>();

            AgentInfo = new AgentChatMetadata()
            {
                AgentId = agentId,
                AgentName = agentName!,
                ThreadId = threadId!,
            };
        }
    }
}
