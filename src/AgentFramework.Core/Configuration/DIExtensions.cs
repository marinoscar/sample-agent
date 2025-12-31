using AgentFramework.Core.Agents;
using AgentFramework.Core.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Configuration
{
    /// <summary>
    /// Provides extension methods for dependency injection configuration of the Agent Framework.
    /// </summary>
    public static class DIExtensions
    {
        /// <summary>
        /// Adds the AgentFactory and its dependencies to the service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="contextFactory">
        /// Optional factory function for creating <see cref="IAgentStoreContext"/> instances.
        /// If not provided, defaults to creating <see cref="SqliteAgentContext"/> instances.
        /// </param>
        /// <returns>The <see cref="IServiceCollection"/> for method chaining.</returns>
        /// <remarks>
        /// This method registers the following services with scoped lifetime:
        /// <list type="bullet">
        /// <item><description><see cref="Func{IAgentStoreContext}"/> - Factory for creating agent store contexts</description></item>
        /// <item><description><see cref="Func{IAgentStore}"/> - Factory for creating agent store services</description></item>
        /// <item><description><see cref="AgentFactory"/> - Main factory for creating and managing agents</description></item>
        /// </list>
        /// </remarks>
        /// <example>
        /// Basic usage with default SQLite context:
        /// <code>
        /// services.AddAgentFactory();
        /// </code>
        /// 
        /// Usage with custom context factory:
        /// <code>
        /// services.AddAgentFactory(() => new CustomAgentContext(connectionString));
        /// </code>
        /// </example>
        public static IServiceCollection AddAgentFactory(this IServiceCollection services, Func<IAgentStoreContext> contextFactory = null)
        {
            // Use default SQLite context if no custom factory is provided
            if (contextFactory == null)
                contextFactory = (() => new SqliteAgentContext());

            // Register the IAgentStoreContext factory
            // This allows consumers to create new context instances on demand
            services.AddScoped<Func<IAgentStoreContext>>(sp =>
            {
                return contextFactory;
            });

            // Register the AgentStoreService factory
            // This provides a way to create IAgentStore instances that use the configured context
            services.AddScoped<Func<IAgentStore>>(c =>
            {
                var agentContextFactory = c.GetRequiredService<Func<IAgentStoreContext>>();
                return () =>
                {
                    return new AgentStoreService(agentContextFactory);
                };
            });

            // Register the AgentFactory with its dependencies
            // The AgentFactory is the primary entry point for creating and managing agents
            services.AddScoped<AgentFactory>(sp =>
            {
                var agentStoreFactory = sp.GetRequiredService<Func<IAgentStore>>();
                var loggerFactory = sp.GetService<ILoggerFactory>();
                return new AgentFactory(agentStoreFactory, loggerFactory);
            });

            return services;
        }
    }
}
