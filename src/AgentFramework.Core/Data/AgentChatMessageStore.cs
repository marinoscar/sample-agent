using Microsoft.Agents.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AgentFramework.Core.Data
{
    public abstract class AgentChatMessageStore : ChatMessageStore
    {

        protected JsonElement SerializedStoreState { get; private set; }
        protected JsonSerializerOptions JsonSerializerOptions { get; private set; } = default!;

        public virtual AgentChatMetadata AgentInfo { get; private set; } = default!;

        protected AgentChatMessageStore(string agentId, string agentName, JsonElement serializedStoreState, JsonSerializerOptions? jsonSerializerOptions = null)
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
