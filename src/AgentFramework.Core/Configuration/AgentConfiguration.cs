using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Configuration
{
    public class AgentConfiguration
    {
        public string Id { get; set; }
        public string Provider { get; set; } // e.g., "OpenAI", "AzureOpenAI", "Anthropic"
        public string Name { get; set; }
        public string Description { get; set; }
        public string Model { get; set; }
        public string Instructions { get; set; }
        public string ToolMode { get; set; }
        public float? Temperature { get; set; }

        public bool PersistConversation { get; set; } = false;
        public string ResponseFormat { get; set; }
        public string ToolList { get; set; }
        public string? AdditinonalSettings { get; set; }

        public AgentConfiguration()
        {
            Id = Guid.NewGuid().ToString("N");
            Instructions = "You are a helpful AI assistant.";
            Temperature = null; // not supported by models gpt-5-nano and gpt-5-mini
            Model = "gpt-5-nano";
            Name = "Default Agent";
            ResponseFormat = ChatResponseFormat.Text.GetType().Name;
            ToolMode = ChatToolMode.Auto.GetType().Name;
            Description = string.Empty;
            ToolList = "web_search,code_interpreter,datetime";
            Provider = "OpenAI";
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
