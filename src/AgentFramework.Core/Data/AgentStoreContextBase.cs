using AgentFramework.Core.Configuration;
using Microsoft.EntityFrameworkCore;

namespace AgentFramework.Core.Data
{
    /// <summary>
    /// Provides a base Entity Framework Core context for agent-related data, including agent messages and
    /// configurations.
    /// </summary>
    /// <remarks>This abstract class defines the core schema and behaviors for storing agent messages and
    /// configurations in a relational database. It must be extended by provider-specific implementations to supply
    /// details such as the SQL data type for unbounded text columns. The context exposes DbSet properties for agent
    /// messages and configurations, and includes a method to ensure the database is created or migrated as needed. This
    /// class is intended for use as the foundation of an agent data store and should not be instantiated
    /// directly.</remarks>
    public abstract class AgentStoreContextBase : DbContext, IAgentStoreContext
    {

        /// <summary>
        /// Creates a new instance of <see cref="AgentStoreContextBase"/>.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        protected AgentStoreContextBase(DbContextOptions options) : base(options) { }

        /// <inheritdoc/>
        public DbSet<AgentMessage> AgentMessages => Set<AgentMessage>();

        /// <inheritdoc/>
        public DbSet<AgentConfiguration> AgentConfigurations => Set<AgentConfiguration>();

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region AgentMessage

            var e = modelBuilder.Entity<AgentMessage>();

            e.ToTable("agent_messages");

            // PK
            e.HasKey(x => x.Id);
            e.Property(x => x.Id)
             .ValueGeneratedOnAdd();

            e.Property(x => x.AgentId)
             .IsRequired()
             .HasMaxLength(100);

            e.Property(x => x.AgentName)
             .IsRequired()
             .HasMaxLength(256);

            // ThreadId (required)
            e.Property(x => x.ThreadId)
             .IsRequired()
             .HasColumnType(UnboundedTextType);

            // 🔹 INDEX ON ThreadId (portable)
            e.HasIndex(x => x.ThreadId)
             .HasDatabaseName("ix_agent_messages_thread_id");

            e.HasIndex(x => x.AgentId)
             .HasDatabaseName("ix_agent_messages_agent_id");

            // Required text columns
            e.Property(x => x.MessageText)
             .IsRequired()
             .HasColumnType(UnboundedTextType);

            e.Property(x => x.SerializedMessage)
             .IsRequired()
             .HasColumnType(UnboundedTextType);

            // Required timestamp
            e.Property(x => x.UtcCreatedAt)
             .IsRequired(); 

            #endregion

            #region AgentConfiguration

            var ac = modelBuilder.Entity<AgentConfiguration>();

            ac.ToTable("agent_configurations");

            // PK
            ac.HasKey(x => x.Id);
            ac.Property(x => x.Id)
              .ValueGeneratedOnAdd();

            // Name with index
            ac.Property(x => x.Name)
              .IsRequired()
              .HasMaxLength(256);

            ac.HasIndex(x => x.Name)
              .HasDatabaseName("ix_agent_configurations_name");

            var typeConverter = new TypeToStringConverter();
            ac.Property(x => x.ResponseFormat)
                .HasConversion(typeConverter)
                .HasMaxLength(1024);

            #endregion
        }

        /// <inheritdoc/>
        public virtual async Task EnsureDatabaseReadyAsync(CancellationToken ct = default)
        {
            if (Database.IsRelational())
            {
                await Database.MigrateAsync(ct);
            }
            else
            {
                await Database.EnsureCreatedAsync(ct);
            }
        }

        /// <summary>
        /// Gets the provider-specific SQL data type used for unbounded text columns.
        /// </summary>
        /// <remarks>
        /// Different database providers use different data types for storing large text without length restrictions:
        /// <list type="bullet">
        /// <item><description>SQL Server: "nvarchar(max)"</description></item>
        /// <item><description>PostgreSQL: "text"</description></item>
        /// <item><description>SQLite: "TEXT"</description></item>
        /// <item><description>MySQL: "longtext"</description></item>
        /// </list>
        /// This property must be overridden in provider-specific implementations to return the appropriate type name.
        /// </remarks>
        protected abstract string UnboundedTextType { get; }
    }
}
