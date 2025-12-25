using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Data
{
    public abstract class AgentStoreContextBase : DbContext, IAgentMessageContext
    {
        protected AgentStoreContextBase(DbContextOptions options) : base(options) { }

        public DbSet<AgentMessage> AgentMessages => Set<AgentMessage>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
        }

        // Provider-specific unbounded text type
        protected abstract string UnboundedTextType { get; }
    }
}
