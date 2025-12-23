using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Data
{
    public class Message
    {
        public ulong Id { get; set; }
        public string ThreadId { get; set; } = default!;
        public string MessageText { get; set; } = default!;
        public string SerializedMessage { get; set; } = default!;
        public DateTime UtcCreatedAt { get; set; }

    }
}
