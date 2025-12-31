using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Configuration
{
    /// <summary>
    /// Represents credential information for an agent, including an API key and additional properties.
    /// </summary>
    public class AgentCredential
    {
        /// <summary>
        /// Gets or sets the API key used for authentication.
        /// </summary>
        public string ApiKey { get; set; } = default!;

        /// <summary>
        /// Gets or sets additional properties that can be used to store custom credential information.
        /// </summary>
        public Dictionary<string, string> ExtendedProperties { get; set; } = new Dictionary<string, string>();
    }
}
