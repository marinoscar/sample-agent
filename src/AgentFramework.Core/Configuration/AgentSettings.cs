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
        public string ApiKey { get { return Settings["ApiKey"]; } set { Settings["ApiKey"] = value; } }
        public string Model { get { return Settings["Model"]; } set { Settings["Model"] = value; } }
        public string Instructions { get { return Settings["Instructions"]; } set { Settings["Instructions"] = value; } }
        public double Temperature { get { return double.Parse(Settings["Temperature"]); } set { Settings["Temperature"] = value.ToString(); } }
    }
}
