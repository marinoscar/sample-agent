using AgentFramework.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Data
{
    /// <summary>
    /// Provides data access services for managing agent messages and configurations in the agent store.
    /// </summary>
    /// <remarks>
    /// This service implements the repository pattern for agent-related data operations,
    /// providing methods for CRUD operations on agent messages and configurations.
    /// </remarks>
    public class AgentStoreService : IAgentStore
    {
        private readonly IAgentStoreContext _db;
        private static bool _isInitialized = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentStoreService"/> class.
        /// </summary>
        /// <param name="createContext">A factory function that creates an instance of <see cref="IAgentStoreContext"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="createContext"/> is null or returns null.</exception>
        public AgentStoreService(Func<IAgentStoreContext> createContext)
        {
            _db = createContext() ?? throw new ArgumentNullException(nameof(createContext));
        }

        /// <inheritdoc/>
        public async Task EnsureStoreIsReadyAsync(CancellationToken ct = default)
        {
            if(_isInitialized) return;
            await _db.EnsureDatabaseReadyAsync(ct);
            _isInitialized = true;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<AgentMessage>> GetByThreadIdAsync(
            AgentChatMetadata agentInfo,
            CancellationToken ct = default)
        {
            ValidateAgentInfo(agentInfo);
            return await _db.AgentMessages
                .AsNoTracking()
                .Where(m => m.AgentId == agentInfo.AgentId && m.ThreadId == agentInfo.ThreadId)
                .OrderBy(m => m.UtcCreatedAt)
                .ToListAsync(ct);
        }

        /// <inheritdoc/>
        public async Task AddRangeAsync(
            AgentChatMetadata agentInfo,
            IEnumerable<AgentMessage> messages,
            CancellationToken ct = default)
        {
            ValidateAgentInfo(agentInfo);
            if (messages is null)
                throw new ArgumentNullException(nameof(messages));

            var list = messages as IList<AgentMessage> ?? messages.ToList();
            if (list.Count == 0) return;

            var now = DateTime.UtcNow;

            foreach (var m in list)
            {
                if (m is null) continue;

                m.ThreadId = agentInfo.ThreadId;
                m.AgentId = agentInfo.AgentId;
                m.AgentName = agentInfo.AgentName;

                if (string.IsNullOrWhiteSpace(m.MessageText))
                    m.MessageText = string.Empty;

                if (string.IsNullOrWhiteSpace(m.SerializedMessage))
                    throw new ArgumentException("SerializedMessage is required.", nameof(messages));

                if (m.UtcCreatedAt == default)
                    m.UtcCreatedAt = now;
            }

            await _db.AgentMessages.AddRangeAsync(list, ct);
            await _db.SaveChangesAsync(ct);
        }

        /// <inheritdoc/>
        public async Task<AgentConfiguration> GetAgentConfigurationByIdAsync(
            string agentId,
            CancellationToken ct = default)
        {
            return await GetAgentConfigurationAsync(i => i.Id == agentId, $"Agent configuration not found for AgentId: {agentId}", ct);
        }

        /// <inheritdoc/>
        public async Task<AgentConfiguration> GetAgentConfigurationByNameAsync(
            string agentName,
            CancellationToken ct = default)
        {
            return await GetAgentConfigurationAsync(i => i.Name == agentName, $"Agent configuration not found for AgentName: {agentName}", ct);
        }

        /// <inheritdoc/>
        public async Task<AgentConfiguration> GetAgentConfigurationAsync(Expression<Func<AgentConfiguration, bool>> expression, string? errorMessage = null, CancellationToken ct = default)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            var config = await _db.AgentConfigurations
                .AsNoTracking()
                .FirstOrDefaultAsync(expression, ct);
            return config ?? throw new InvalidOperationException(errorMessage ?? $"Agent configuration not found for the specified criteria.");
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<AgentConfiguration>> GetAllAgentConfigurationsAsync(
            CancellationToken ct = default)
        {
            return await _db.AgentConfigurations
                .AsNoTracking()
                .ToListAsync(ct);
        }

        /// <inheritdoc/>
        public async Task<AgentConfiguration> AddOrUpdateAsync(
            AgentConfiguration agentConfiguration,
            CancellationToken ct = default)
        {
            if (agentConfiguration is null)
                throw new ArgumentNullException(nameof(agentConfiguration));
            var existingConfig = await _db.AgentConfigurations
                .FirstOrDefaultAsync(c => c.Id == agentConfiguration.Id, ct);
            if (existingConfig is null)
            {
                await _db.AgentConfigurations.AddAsync(agentConfiguration, ct);
            }
            else
            {
                _db.Entry(existingConfig).CurrentValues.SetValues(agentConfiguration);
            }
            await _db.SaveChangesAsync(ct);
            return agentConfiguration;
        }

        /// <summary>
        /// Validates the agent chat metadata to ensure required fields are present.
        /// </summary>
        /// <param name="agentInfo">The agent chat metadata to validate.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="agentInfo"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when AgentId or ThreadId is null or whitespace.</exception>
        protected virtual void ValidateAgentInfo(AgentChatMetadata agentInfo)
        {
            if (agentInfo is null)
                throw new ArgumentNullException(nameof(agentInfo));
            if (string.IsNullOrWhiteSpace(agentInfo.AgentId))
                throw new ArgumentException("AgentId is required.", nameof(agentInfo));
            if (string.IsNullOrWhiteSpace(agentInfo.ThreadId))
                throw new ArgumentException("ThreadId is required.", nameof(agentInfo));
        }
    }
}
