using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Configuration
{
    public class OpenAISettings : AgentSettings
    {
        public bool EnableWebSearch { get { return bool.Parse(Settings[nameof(EnableWebSearch)]); } set { Settings[nameof(EnableWebSearch)] = value.ToString(); } }
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
                Instructions = instructions
            };
        }
    }
}
