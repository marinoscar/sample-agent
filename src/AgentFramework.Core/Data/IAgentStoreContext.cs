using AgentFramework.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Data
{
    /// <summary>
    /// Defines the contract for the agent store database context.
    /// Provides access to agent-related data collections and database operations.
    /// </summary>
    public interface IAgentStoreContext
    {
        /// <summary>
        /// Gets the collection of agent messages stored in the database.
        /// </summary>
        DbSet<AgentMessage> AgentMessages { get; }

        /// <summary>
        /// Gets the collection of agent configurations stored in the database.
        /// </summary>
        DbSet<AgentConfiguration> AgentConfigurations { get; }

        /// <summary>
        /// Asynchronously saves all changes made in this context to the database.
        /// </summary>
        /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
        Task<int> SaveChangesAsync(CancellationToken ct = default);

        /// <summary>
        /// Ensures the database is created and ready for use, applying any pending migrations if necessary.
        /// </summary>
        /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task EnsureDatabaseReadyAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets the database facade for performing database-level operations such as managing transactions or executing raw SQL commands.
        /// </summary>
        public DatabaseFacade Database { get; }
    }
}
