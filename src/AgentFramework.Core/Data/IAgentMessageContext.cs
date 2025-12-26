using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Data
{
    public interface IAgentMessageContext
    {
        DbSet<AgentMessage> AgentMessages { get; }

        Task<int> SaveChangesAsync(CancellationToken ct = default);

        Task EnsureDatabaseReadyAsync(CancellationToken ct = default);

        public DatabaseFacade Database { get; }
    }
}
