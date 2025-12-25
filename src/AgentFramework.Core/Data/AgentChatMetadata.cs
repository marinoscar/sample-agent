using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Data
{
    public class AgentChatMetadata
    {
        public string AgentId { get; set; } = null!;
        public string AgentName { get; set; } = null!;
        public string ThreadId { get; set; } = null!;
    }
}
