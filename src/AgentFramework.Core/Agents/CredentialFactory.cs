using AgentFramework.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Agents
{
    public class CredentialFactory
    {
        public AgentCredential GetFromProvider(string providerName)
        {
            var normalizedProviderName = providerName?.Trim().ToLowerInvariant();
            return normalizedProviderName switch
            {
                "openai" => GetForOpenAI(),
                "azureopenai" => GetForAzureOpenAI(),
                "anthropic" => GetForAnthropic(),
                "gemini" => GetForGemini(),
                _ => throw new NotSupportedException($"Provider '{providerName}' is not supported.")
            };
        }

        public AgentCredential GetForOpenAI()
        {
            var key = Env.GetOptional("OPENAI_API_KEY");
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("OPENAI_API_KEY environment variable is required");
            }
            return new AgentCredential
            {
                ApiKey = key,
                ExtendedProperties = new Dictionary<string, string>()
            };
        }

        public AgentCredential GetForAzureOpenAI()
        {
            var key = Env.GetOptional("AZURE_OPENAI_KEY");
            var endpoint = Env.GetOptional("AZURE_OPENAI_ENDPOINT");
            var deploymentName = Env.GetOptional("AZURE_OPENAI_DEPLOYMENT_NAME");
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(deploymentName))
            {
                throw new InvalidOperationException("AZURE_OPENAI_KEY, AZURE_OPENAI_ENDPOINT, and AZURE_OPENAI_DEPLOYMENT_NAME environment variables are required");
            }
            return new AgentCredential
            {
                ApiKey = key,
                ExtendedProperties = new Dictionary<string, string>
                {
                    { "Endpoint", endpoint },
                    { "DeploymentName", deploymentName }
                }
            };
        }

        public AgentCredential GetForAnthropic()
        {
            var key = Env.GetOptional("ANTHROPIC_API_KEY");
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("ANTHROPIC_API_KEY environment variable is required");
            }
            return new AgentCredential
            {
                ApiKey = key,
                ExtendedProperties = new Dictionary<string, string>()
            };
        }

        public AgentCredential GetForGemini()
        {
            var key = Env.GetOptional("GOOGLE_GEMINI_API_KEY");
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("GOOGLE_GEMINI_API_KEY environment variable is required");
            }
            return new AgentCredential
            {
                ApiKey = key,
                ExtendedProperties = new Dictionary<string, string>()
            };
        }

    }
}
