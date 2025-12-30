using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Data
{
    /// <summary>
    /// Represents metadata for an agent chat session, including agent identification and thread tracking information.
    /// </summary>
    public class AgentChatMetadata
    {
        /// <summary>
        /// Gets or sets the unique identifier for the agent.
        /// </summary>
        public string AgentId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the display name of the agent.
        /// </summary>
        public string AgentName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier for the conversation thread.
        /// </summary>
        public string ThreadId { get; set; } = null!;
    }
}
