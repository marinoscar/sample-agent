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

        public AgentMessageStoreService(IAgentMessageContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<IReadOnlyList<AgentMessage>> GetByThreadIdAsync(
            string threadId,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(threadId))
                throw new ArgumentException("ThreadId is required.", nameof(threadId));

            return await _db.AgentMessages
                .AsNoTracking()
                .Where(m => m.ThreadId == threadId)
                .OrderBy(m => m.UtcCreatedAt)
                .ToListAsync(ct);
        }

        public async Task AddRangeAsync(
            string threadId,
            IEnumerable<AgentMessage> messages,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(threadId))
                throw new ArgumentException("ThreadId is required.", nameof(threadId));
            if (messages is null)
                throw new ArgumentNullException(nameof(messages));

            var list = messages as IList<AgentMessage> ?? messages.ToList();
            if (list.Count == 0) return;

            var now = DateTime.UtcNow;

            foreach (var m in list)
            {
                if (m is null) continue;

                m.ThreadId = threadId;

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
    }
}
