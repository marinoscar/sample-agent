using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Data
{
    /// <summary>
    /// Provides a SQLite-based implementation of the agent message store context.
    /// This context manages the persistence of agent messages and related data using a SQLite database.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This implementation uses Entity Framework Core with SQLite as the database provider.
    /// The database connection can be configured via the AGENT_SQLITE_STORE environment variable.
    /// If not specified, a default database file "agent_messages.db" will be created in the application's base directory.
    /// </para>
    /// <para>
    /// The database schema is automatically created on first use through the <see cref="EnsureDatabaseReadyAsync"/> method,
    /// which is called during context initialization. This ensures the database is ready before any operations are performed.
    /// </para>
    /// <para>
    /// This class is sealed and cannot be inherited. It extends <see cref="AgentStoreContextBase"/> to provide
    /// SQLite-specific implementations of database operations.
    /// </para>
    /// </remarks>
    public sealed class SqliteAgentContext : AgentStoreContextBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqliteAgentContext"/> class with the specified connection string.
        /// </summary>
        /// <param name="connectionString">The SQLite connection string used to connect to the database.</param>
        /// <remarks>
        /// The constructor ensures the database is created and ready before returning by calling <see cref="EnsureDatabaseReadyAsync"/>.
        /// </remarks>
        public SqliteAgentContext(string connectionString)
            : base(new DbContextOptionsBuilder<SqliteAgentContext>()
                  .UseSqlite(connectionString)
                  .Options)
        {
            this.EnsureDatabaseReadyAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqliteAgentContext"/> class using the default connection string.
        /// </summary>
        /// <remarks>
        /// The connection string is determined by the <see cref="GetConnectionString"/> method, which checks
        /// the AGENT_SQLITE_STORE environment variable or uses a default database location.
        /// </remarks>
        public SqliteAgentContext() : this(GetConnectionString())
        {

        }

        
        private static string GetConnectionString()
        {
            var connectionString = Env.GetOptional("AGENT_SQLITE_STORE");
            if (string.IsNullOrEmpty(connectionString))
            {
                var dbPath = Path.Combine(AppContext.BaseDirectory, "agent_messages.db");
                connectionString = $"Data Source={dbPath}";
            }
            return connectionString;
        }

        /// <summary>
        /// Gets the SQLite data type used for unbounded text columns.
        /// </summary>
        /// <value>Returns "TEXT" which is the SQLite data type for text fields without length restrictions.</value>
        /// <remarks>
        /// This property is used by the base context to configure entity properties that require unlimited text storage.
        /// </remarks>
        protected override string UnboundedTextType => "TEXT";

        /// <summary>
        /// Ensures that the SQLite database is created and ready for use.
        /// </summary>
        /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <remarks>
        /// <para>
        /// This method calls <see cref="DatabaseFacade.EnsureCreatedAsync"/> to create the database and all its tables
        /// if they do not already exist. This method does NOT apply migrations - it creates the database schema
        /// based on the current model.
        /// </para>
        /// <para>
        /// <strong>Note:</strong> If you are using EF Core migrations, you should use the migrations system instead
        /// of this method to ensure proper version control of your database schema.
        /// </para>
        /// </remarks>
        public override async Task EnsureDatabaseReadyAsync(CancellationToken ct = default)
        {
            await Database.EnsureCreatedAsync(ct);
        }

    }
}
