using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Data
{
    public sealed class PostgresAgentMessageContext : AgentStoreContextBase
    {
        public PostgresAgentMessageContext(string connectionString)
            : base(new DbContextOptionsBuilder<PostgresAgentMessageContext>()
                  .UseNpgsql(connectionString)
                  .Options)
        { }

        public PostgresAgentMessageContext() : this(GetConnectionString())
        {
            
        }

       private static string GetConnectionString()
        {
            var host = Environment.GetEnvironmentVariable("AGENT_DB_HOST") ?? throw new InvalidOperationException("AGENT_DB_HOST environment variable is required");
            var port = Environment.GetEnvironmentVariable("AGENT_DB_PORT") ?? throw new InvalidOperationException("AGENT_DB_PORT environment variable is required");
            var database = Environment.GetEnvironmentVariable("AGENT_DB_NAME") ?? throw new InvalidOperationException("AGENT_DB_NAME environment variable is required");
            var username = Environment.GetEnvironmentVariable("AGENT_DB_USER") ?? throw new InvalidOperationException("AGENT_DB_USER environment variable is required");
            var password = Environment.GetEnvironmentVariable("AGENT_DB_PASSWORD") ?? throw new InvalidOperationException("AGENT_DB_PASSWORD environment variable is required");
            var connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";
            return connectionString;
        }

        protected override string UnboundedTextType => "text";
    }
}
