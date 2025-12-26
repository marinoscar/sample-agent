using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Data
{
    public sealed class SqliteAgentMessageContext : AgentStoreContextBase
    {
        public SqliteAgentMessageContext(string connectionString)
            : base(new DbContextOptionsBuilder<SqliteAgentMessageContext>()
                  .UseSqlite(connectionString)
                  .Options)
        {
            this.EnsureDatabaseReadyAsync().GetAwaiter().GetResult();
        }

        public SqliteAgentMessageContext() : this(GetConnectionString())
        {
            
        }

        private static string GetConnectionString()
        {
            var dbPath = Path.Combine(AppContext.BaseDirectory, "agent_messages.db");
            var connectionString = $"Data Source={dbPath}";
            return connectionString;
        }

        protected override string UnboundedTextType => "TEXT";

        public override async Task EnsureDatabaseReadyAsync(CancellationToken ct = default)
        {
            await Database.EnsureCreatedAsync(ct);
        }

    }
}
