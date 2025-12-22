using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Configuration
{
    public class OpenAISettings : AgentSettings
    {
        public bool EnableCodeInterpreter { get { return bool.Parse(Settings[nameof(EnableCodeInterpreter)]); } set { Settings[nameof(EnableCodeInterpreter)] = value.ToString(); } }
        public bool EnableWebSearch { get { return bool.Parse(Settings[nameof(EnableWebSearch)]); } set { Settings[nameof(EnableWebSearch)] = value.ToString(); } }
        public bool EnableImageGeneration { get { return bool.Parse(Settings[nameof(EnableImageGeneration)]); } set { Settings[nameof(EnableImageGeneration)] = value.ToString(); } }
        public static OpenAISettings Create(string instructions, string model = "gpt-4o-mini", double tem = 7.0d)
        {
            var key = Environment.GetEnvironmentVariable("OpenAIKey", EnvironmentVariableTarget.User);
            if(string.IsNullOrEmpty(key)) throw new Exception("OpenAIKey environment variable not set.");
            if(string.IsNullOrEmpty(instructions)) instructions = "You are a helpful assistant.";
            return new OpenAISettings
            {
                Model = model,
                Temperature = tem,
                ApiKey = key,
                Instructions = instructions,
                EnableCodeInterpreter = true,
                EnableImageGeneration = false,
                EnableWebSearch = true
            };
        }

        public static OpenAISettings FromAgentSettings(AgentSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            var openAISettings = new OpenAISettings
            {
                Model = settings.Model,
                Temperature = settings.Temperature,
                ApiKey = settings.ApiKey,
                Instructions = settings.Instructions,
                EnableCodeInterpreter = true,
                EnableImageGeneration = false,
                EnableWebSearch = true
            };

            // Copy any additional settings from the base Settings dictionary
            foreach (var kvp in settings.Settings)
            {
                if (!openAISettings.Settings.ContainsKey(kvp.Key))
                {
                    openAISettings.Settings[kvp.Key] = kvp.Value;
                }
            }

            return openAISettings;
        }
    }
}
