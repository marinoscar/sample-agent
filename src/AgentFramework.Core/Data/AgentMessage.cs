using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Data
{
    /// <summary>
    /// Represents a message sent by an agent within a conversation thread.
    /// </summary>
    public class AgentMessage
    {
        /// <summary>
        /// Gets or sets the unique identifier for the message.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the agent that sent the message.
        /// </summary>
        public string AgentId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the unique identifier of the conversation thread this message belongs to.
        /// </summary>
        public string ThreadId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the name of the agent that sent the message.
        /// </summary>
        public string AgentName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the text content of the message.
        /// </summary>
        public string MessageText { get; set; } = default!;

        /// <summary>
        /// Gets or sets the serialized representation of the complete message object.
        /// </summary>
        public string SerializedMessage { get; set; } = default!;

        /// <summary>
        /// Gets or sets the UTC timestamp when the message was created.
        /// </summary>
        public DateTime UtcCreatedAt { get; set; }
    }
}
