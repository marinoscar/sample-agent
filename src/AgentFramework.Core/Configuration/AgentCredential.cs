using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Configuration
{
    public class AgentCredential
    {
        public string ApiKey { get; set; } = default!;
        public Dictionary<string, string> ExtendedProperties { get; set; } = new Dictionary<string, string>();
    }
}
