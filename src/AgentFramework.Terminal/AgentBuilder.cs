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

namespace AgentFramework.Terminal
{
    /// <summary>
    /// Helps initialize agent dependencies and build the agent
    /// </summary>
    public static class AgentBuilder
    {



        public static HostApplicationBuilder AddAgentFactory(this HostApplicationBuilder builder, Func<IAgentStoreContext> contextFactory = null)
        {
            
            if(contextFactory == null)
                contextFactory = (() => new SqliteAgentContext());

            // Register the IAgentStoreContext factory
            builder.Services.AddScoped<Func<IAgentStoreContext>>(sp =>
            {
                return contextFactory;
            });


            // Register the AgentStoreService factory
            builder.Services.AddScoped<Func<IAgentStore>>(c =>
            {
                var agentContextFactory = c.GetRequiredService<Func<IAgentStoreContext>>();
                return () =>
                {
                    return new AgentStoreService(agentContextFactory);
                };
            });

            // Register the AgentFactory with its dependencies
            builder.Services.AddScoped<AgentFactory>(sp =>
            {
                var agentStoreFactory = sp.GetRequiredService<Func<IAgentStore>>();
                var loggerFactory = sp.GetService<ILoggerFactory>();
                return new AgentFactory(agentStoreFactory, loggerFactory);
            });

            return builder;
        }
    }
}
