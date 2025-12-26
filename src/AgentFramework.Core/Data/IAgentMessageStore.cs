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
            AgentChatMetadata agentInfo,
            CancellationToken ct = default);

        Task AddRangeAsync(
            AgentChatMetadata agentInfo,
            IEnumerable<AgentMessage> messages,
            CancellationToken ct = default);

        Task EnsureStoreIsReadyAsync(CancellationToken ct = default);
    }
}
