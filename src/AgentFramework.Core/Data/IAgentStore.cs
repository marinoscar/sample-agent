using AgentFramework.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Data
{
    /// <summary>
    /// Defines the contract for storing and retrieving agent messages and configurations.
    /// Provides a unified interface for managing agent chat threads, messages, and agent configuration metadata.
    /// </summary>
    public interface IAgentStore
    {
        /// <summary>
        /// Retrieves all messages associated with a specific agent chat thread.
        /// </summary>
        /// <param name="agentInfo">The metadata identifying the agent chat thread.</param>
        /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A read-only list of agent messages in the specified thread, ordered chronologically.</returns>
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
        /// Retrieves an agent configuration by its unique identifier.
        /// </summary>
        /// <param name="agentId">The unique identifier of the agent.</param>
        /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>The agent configuration if found; otherwise, null.</returns>
        Task<AgentConfiguration> GetAgentConfigurationByIdAsync(
            string agentId,
            CancellationToken ct = default);

        /// <summary>
        /// Retrieves an agent configuration by its name.
        /// </summary>
        /// <param name="agentName">The name of the agent.</param>
        /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>The agent configuration if found; otherwise, null.</returns>
        Task<AgentConfiguration> GetAgentConfigurationByNameAsync(
            string agentName,
            CancellationToken ct = default);

        /// <summary>
        /// Retrieves all agent configurations stored in the system.
        /// </summary>
        /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A read-only list of all agent configurations.</returns>
        Task<IReadOnlyList<AgentConfiguration>> GetAllAgentConfigurationsAsync(
            CancellationToken ct = default);

        /// <summary>
        /// Adds a new agent configuration or updates an existing one.
        /// If an agent with the same identifier exists, it will be updated; otherwise, a new configuration is created.
        /// </summary>
        /// <param name="agentConfiguration">The agent configuration to add or update.</param>
        /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>The added or updated agent configuration.</returns>
        Task<AgentConfiguration> AddOrUpdateAsync(
            AgentConfiguration agentConfiguration,
            CancellationToken ct = default);

        /// <summary>
        /// Ensures the underlying storage infrastructure is initialized and ready for operations.
        /// This method should be called before performing any store operations to guarantee proper setup.
        /// </summary>
        /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous initialization operation.</returns>
        Task EnsureStoreIsReadyAsync(CancellationToken ct = default);
    }
}
