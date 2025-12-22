using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Configuration
{
    public class AgentSettings
    {
        public IDictionary<string, string> Settings { get; set; } = new Dictionary<string, string>();
        public string ApiKey { get { return Settings[nameof(ApiKey)]; } set { Settings[nameof(ApiKey)] = value; } }
        public string Model { get { return Settings[nameof(Model)]; } set { Settings[nameof(Model)] = value; } }
        public string Instructions { get { return Settings[nameof(Instructions)]; } set { Settings[nameof(Instructions)] = value; } }
        public double Temperature { get { return double.Parse(Settings[nameof(Temperature)]); } set { Settings[nameof(Temperature)] = value.ToString(); } }

        public string Name { get { return Settings.ContainsKey(nameof(Name)) ? Settings[nameof(Name)] : string.Empty; } set { Settings[nameof(Name)] = value; } }
        public string Description { get { return Settings.ContainsKey(nameof(Description)) ? Settings[nameof(Description)] : string.Empty; } set { Settings[nameof(Description)] = value; } }
        public string Id { get { return Settings.ContainsKey(nameof(Id)) ? Settings[nameof(Id)] : string.Empty; } set { Settings[nameof(Id)] = value; } }
    }
}
