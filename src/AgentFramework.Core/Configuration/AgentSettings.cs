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
        public string ApiKey { get; set; }
        public string Model { get; set; }
        public string Instructions { get; set; }
        public double Temperature { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
        public string ToolMode { get; set; }
        public string ResponseFormat { get; set; }
        public string ToolList { get; set; }

        public AgentSettings()
        {
            Id = Guid.NewGuid().ToString("N");
            Instructions = "You are a helpful AI assistant.";
            Temperature = 0.7d;
            Model = "gpt-5-nano";
            Name = "Default Agent";
            ResponseFormat = ChatResponseFormat.Text.GetType().Name;
            ToolMode = ChatToolMode.Auto.GetType().Name;
            ApiKey = string.Empty;
            Description = string.Empty;
            ToolList = string.Empty;
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
