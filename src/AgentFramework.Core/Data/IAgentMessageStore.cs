using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Data
{
    public interface IAgentMessageStore
    {
        Task<IReadOnlyList<AgentMessage>> GetByThreadIdAsync(
            string threadId,
            CancellationToken ct = default);

        Task AddRangeAsync(
            string threadId,
            IEnumerable<AgentMessage> messages,
            CancellationToken ct = default);
    }
}
