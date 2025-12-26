using Microsoft.Extensions.AI;
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
        public string ToolMode { get { return Settings.ContainsKey(nameof(ToolMode)) ? Settings[nameof(ToolMode)] : string.Empty; } set { Settings[nameof(ToolMode)] = value; } }

        public string ResponseFormat { get { return Settings.ContainsKey(nameof(ResponseFormat)) ? Settings[nameof(ResponseFormat)] : string.Empty; } set { Settings[nameof(ResponseFormat)] = value; } }

        public string ToolList { get { return Settings.ContainsKey(nameof(ToolList)) ? Settings[nameof(ToolList)] : string.Empty; } set { Settings[nameof(ToolList)] = value; } }

        public AgentSettings()
        {
            Id = Guid.NewGuid().ToString("N");
            Instructions = "You are a helpful AI assistant.";
            Temperature = 0.7d;
            Model = "gpt-5-nano";
            Name = "Default Agent";
            ResponseFormat = ChatResponseFormat.Text.GetType().Name;
            ToolMode = ChatToolMode.Auto.GetType().Name;
        }

        public ChatToolMode GetToolMode()
        {
            if (string.IsNullOrEmpty(ToolMode))
            {
                return ChatToolMode.Auto;
            }

            return ToolMode switch
            {
                nameof(ChatToolMode.Auto) => ChatToolMode.Auto,
                nameof(ChatToolMode.RequireAny) => ChatToolMode.RequireAny,
                _ => ChatToolMode.Auto
            };
        }

        public ChatResponseFormat GetResponseFormat()
        {
            if (string.IsNullOrEmpty(ResponseFormat))
            {
                return ChatResponseFormat.Text;
            }

            return ResponseFormat switch
            {
                nameof(ChatResponseFormat.Text) => ChatResponseFormat.Text,
                nameof(ChatResponseFormat.Json) => ChatResponseFormat.Json,
                _ => ChatResponseFormat.Text
            };
        }
    }
}
