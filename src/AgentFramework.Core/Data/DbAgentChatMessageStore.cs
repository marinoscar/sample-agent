using AgentFramework.Core.Agents;
using AgentFramework.Core.Configuration;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

/// <summary>
/// Provides a database-backed implementation of <see cref="AgentChatMessageStoreBase"/> that persists chat messages using an <see cref="IAgentMessageStore"/>.
/// </summary>
namespace AgentFramework.Core.Data
{
    /// <summary>
    /// Provides a database-backed implementation of <see cref="AgentChatMessageStoreBase"/> that persists chat messages using an <see cref="IAgentMessageStore"/>.
    /// </summary>
    public class DbAgentChatMessageStore : AgentChatMessageStoreBase
    {

        private readonly Func<IAgentMessageStore> _agentMessageStoreFactory;
        private readonly IAgentMessageStore _agentMessageStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbAgentChatMessageStore"/> class.
        /// </summary>
        /// <param name="agentMessageStoreFactory">Factory function to create an <see cref="IAgentMessageStore"/> instance.</param>
        /// <param name="agentId">The unique identifier of the agent.</param>
        /// <param name="agentName">The name of the agent.</param>
        /// <param name="serializedStoreState">The serialized state of the store.</param>
        /// <param name="jsonSerializerOptions">Optional JSON serializer options.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="agentMessageStoreFactory"/> is null.</exception>
        public DbAgentChatMessageStore(Func<IAgentMessageStore> agentMessageStoreFactory, string agentId, string agentName, JsonElement serializedStoreState, JsonSerializerOptions? jsonSerializerOptions = null) 
            : base(agentId, agentName, serializedStoreState, jsonSerializerOptions)
        {
            _agentMessageStoreFactory = agentMessageStoreFactory ?? throw new ArgumentNullException(nameof(agentMessageStoreFactory));
            _agentMessageStore = _agentMessageStoreFactory();
            EnsureStoreIsReady();
        }

        /// <inheritdoc />
        public override async Task AddMessagesAsync(IEnumerable<ChatMessage> messages, CancellationToken cancellationToken = default)
        {
            await _agentMessageStore.AddRangeAsync(
                AgentInfo,
                messages.Select(m => m.ToAgentMessage(AgentInfo)),
                cancellationToken);
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<ChatMessage>> GetMessagesAsync(CancellationToken cancellationToken = default)
        {
            return await _agentMessageStore
                .GetByThreadIdAsync(AgentInfo, cancellationToken)
                .ContinueWith(t => t.Result.Select(m => m.ToChatMessage()), cancellationToken);
        }

        /// <inheritdoc />
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
            return JsonSerializer.SerializeToElement(AgentInfo.ThreadId, jsonSerializerOptions);
        }

        /// <summary>
        /// Ensures that the underlying message store is ready for operations by calling <see cref="IAgentMessageStore.EnsureStoreIsReadyAsync"/>.
        /// </summary>
        private void EnsureStoreIsReady()
        {
            _agentMessageStore.EnsureStoreIsReadyAsync().GetAwaiter().GetResult();
        }
    }
}
