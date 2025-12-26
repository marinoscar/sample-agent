using Microsoft.Agents.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Data
{
    public abstract class AgentChatMessageStore : ChatMessageStore
    {
        public AgentChatMetadata AgentInfo { get; set; } = default!;
    }
}
