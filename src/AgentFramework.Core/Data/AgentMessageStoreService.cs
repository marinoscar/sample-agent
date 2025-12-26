using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Data
{
    public class AgentMessageStoreService : IAgentMessageStore
    {
        private readonly IAgentMessageContext _db;
        private static bool _isInitialized = false;

        public AgentMessageStoreService(Func<IAgentMessageContext> createContext)
        {
            _db = createContext() ?? throw new ArgumentNullException(nameof(createContext));
        }

        public async Task EnsureStoreIsReadyAsync(CancellationToken ct = default)
        {
            if(_isInitialized) return;
            await _db.EnsureDatabaseReadyAsync(ct);
            _isInitialized = true;
        }

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
                    throw new ArgumentException("MessageText is required.", nameof(messages));

                if (string.IsNullOrWhiteSpace(m.SerializedMessage))
                    throw new ArgumentException("SerializedMessage is required.", nameof(messages));

                if (m.UtcCreatedAt == default)
                    m.UtcCreatedAt = now;
            }

            await _db.AgentMessages.AddRangeAsync(list, ct);
            await _db.SaveChangesAsync(ct);
        }

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
