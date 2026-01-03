using AgentFramework.Core.Agents;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Workflows
{
    /// <summary>
    /// Provides the abstract base class for agent-based workflows.
    /// </summary>
    /// <remarks>
    /// This class serves as the foundation for creating custom workflows that leverage the AgentFactory
    /// to instantiate and coordinate agents. Each workflow must have a unique identifier and can optionally
    /// have a descriptive name for better identification and logging purposes.
    /// </remarks>
    public abstract class AgentWorkflowBase
    {
        /// <summary>
        /// Gets the agent factory used to create and manage agents within this workflow.
        /// </summary>
        /// <value>
        /// An <see cref="AgentFactory"/> instance that provides agent creation capabilities.
        /// </value>
        public AgentFactory Factory { get; init; }

        /// <summary>
        /// Gets the logger instance for this workflow.
        /// </summary>
        /// <value>
        /// An <see cref="ILogger"/> instance configured with the workflow's ID as the category name.
        /// </value>
        public ILogger Logger { get; init; }

        /// <summary>
        /// Gets the unique identifier for this workflow.
        /// </summary>
        /// <value>
        /// A non-empty string that uniquely identifies this workflow instance.
        /// </value>
        public string Id { get; init; }

        /// <summary>
        /// Gets the descriptive name of this workflow.
        /// </summary>
        /// <value>
        /// A string containing the workflow's display name.
        /// </value>
        public string Name { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentWorkflowBase"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for the workflow. Cannot be null or empty.</param>
        /// <param name="name">The descriptive name of the workflow.</param>
        /// <param name="agentFactory">The agent factory to use for creating agents. Cannot be null.</param>
        /// <param name="loggerFactory">The logger factory to create the workflow logger. Cannot be null.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="id"/> is null or empty, or when <paramref name="agentFactory"/> 
        /// or <paramref name="loggerFactory"/> is null.
        /// </exception>
        protected AgentWorkflowBase(string id, string name, AgentFactory agentFactory, ILoggerFactory loggerFactory)
        {
            if(string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));

            Id = id;
            Name = name;
            Factory = agentFactory ?? throw new ArgumentNullException(nameof(agentFactory));
            Logger = loggerFactory?.CreateLogger(id) ?? throw new ArgumentNullException(nameof(loggerFactory));

        }

        /// <summary>
        /// When overridden in a derived class, builds and returns the workflow configuration.
        /// </summary>
        /// <returns>
        /// A <see cref="Workflow"/> instance representing the complete workflow definition.
        /// </returns>
        /// <remarks>
        /// Implementers should use the <see cref="Factory"/> to create required agents and
        /// configure the workflow steps, transitions, and execution logic.
        /// </remarks>
        public abstract Workflow Build();
    }
}
