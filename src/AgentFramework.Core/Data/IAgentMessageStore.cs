using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Data
{
    /// <summary>
    /// Defines the contract for storing and retrieving agent messages associated with chat threads.
    /// </summary>
    public interface IAgentMessageStore
    {
        /// <summary>
        /// Retrieves all messages associated with a specific agent chat thread.
        /// </summary>
        /// <param name="agentInfo">The metadata identifying the agent chat thread.</param>
        /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A read-only list of agent messages in the specified thread.</returns>
        Task<IReadOnlyList<AgentMessage>> GetByThreadIdAsync(
            AgentChatMetadata agentInfo,
            CancellationToken ct = default);

        /// <summary>
        /// Adds multiple messages to the store for a specific agent chat thread.
        /// </summary>
        /// <param name="agentInfo">The metadata identifying the agent chat thread.</param>
        /// <param name="messages">The collection of messages to add to the store.</param>
        /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddRangeAsync(
            AgentChatMetadata agentInfo,
            IEnumerable<AgentMessage> messages,
            CancellationToken ct = default);

        /// <summary>
        /// Ensures the underlying message store is initialized and ready for operations.
        /// </summary>
        /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task EnsureStoreIsReadyAsync(CancellationToken ct = default);
    }
}
