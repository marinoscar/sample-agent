using AgentFramework.Core.Agents;
using AgentFramework.Core.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        public static HostApplicationBuilder InitializeAgentFactory(this HostApplicationBuilder builder)
        {
            // Register the AgentStoreService factory
            builder.Services.AddScoped<Func<IAgentStore>>((c) =>
            {
                return () =>
                {
                    return new AgentStoreService(() => new SqliteAgentMessageContext());
                };
            });

            // Register the AgentFactory with its dependencies
            builder.Services.AddScoped<AgentFactory>(sp =>
            {
                var agentStoreFactory = sp.GetRequiredService<Func<IAgentStore>>();
                var loggerFactory = sp.GetService<Microsoft.Extensions.Logging.ILoggerFactory>();
                return new AgentFactory(agentStoreFactory, loggerFactory);
            });

            return builder;
        }
    }
}
