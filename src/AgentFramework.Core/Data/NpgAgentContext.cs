using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Data
{
    /// <summary>
    /// PostgreSQL implementation of the agent message store context.
    /// </summary>
    /// <remarks>
    /// This context configures Entity Framework Core to use PostgreSQL as the database provider.
    /// Connection details are retrieved from environment variables:
    /// <list type="bullet">
    /// <item><description>AGENT_DB_HOST - Database server hostname</description></item>
    /// <item><description>AGENT_DB_PORT - Database server port</description></item>
    /// <item><description>AGENT_DB_NAME - Database name</description></item>
    /// <item><description>AGENT_DB_USER - Database username</description></item>
    /// <item><description>AGENT_DB_PASSWORD - Database password</description></item>
    /// </list>
    /// </remarks>
    public sealed class NpgAgentContext : AgentStoreContextBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NpgAgentContext"/> class with the specified connection string.
        /// </summary>
        /// <param name="connectionString">The PostgreSQL connection string.</param>
        public NpgAgentContext(string connectionString)
            : base(new DbContextOptionsBuilder<NpgAgentContext>()
                  .UseNpgsql(connectionString)
                  .Options)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NpgAgentContext"/> class using environment variables for configuration.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when any required environment variable is not set.</exception>
        public NpgAgentContext() : this(GetConnectionString())
        {
            
        }

        /// <summary>
        /// Constructs a PostgreSQL connection string from environment variables.
        /// </summary>
        /// <returns>A formatted PostgreSQL connection string.</returns>
        /// <exception cref="InvalidOperationException">Thrown when any required environment variable is not set.</exception>
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

        /// <inheritdoc />
        protected override string UnboundedTextType => "text";
    }
}
