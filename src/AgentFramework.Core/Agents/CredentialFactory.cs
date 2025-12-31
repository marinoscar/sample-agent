using AgentFramework.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Agents
{
    /// <summary>
    /// Factory for creating agent credentials from various AI service providers.
    /// </summary>
    public class CredentialFactory
    {
        /// <summary>
        /// Gets credentials for the specified provider by reading environment variables.
        /// </summary>
        /// <param name="providerName">The name of the AI service provider (e.g., "openai", "azureopenai", "anthropic", "gemini").</param>
        /// <returns>An <see cref="AgentCredential"/> instance configured for the specified provider.</returns>
        /// <exception cref="NotSupportedException">Thrown when the provider name is not recognized.</exception>
        /// <exception cref="InvalidOperationException">Thrown when required environment variables are missing.</exception>
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

        /// <summary>
        /// Gets credentials for OpenAI from the OPENAI_API_KEY environment variable.
        /// </summary>
        /// <returns>An <see cref="AgentCredential"/> instance configured for OpenAI.</returns>
        /// <exception cref="InvalidOperationException">Thrown when OPENAI_API_KEY environment variable is not set.</exception>
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

        /// <summary>
        /// Gets credentials for Azure OpenAI from environment variables.
        /// Requires AZURE_OPENAI_KEY, AZURE_OPENAI_ENDPOINT, and AZURE_OPENAI_DEPLOYMENT_NAME.
        /// </summary>
        /// <returns>An <see cref="AgentCredential"/> instance configured for Azure OpenAI.</returns>
        /// <exception cref="InvalidOperationException">Thrown when required Azure OpenAI environment variables are not set.</exception>
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

        /// <summary>
        /// Gets credentials for Anthropic from the ANTHROPIC_API_KEY environment variable.
        /// </summary>
        /// <returns>An <see cref="AgentCredential"/> instance configured for Anthropic.</returns>
        /// <exception cref="InvalidOperationException">Thrown when ANTHROPIC_API_KEY environment variable is not set.</exception>
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

        /// <summary>
        /// Gets credentials for Google Gemini from the GOOGLE_GEMINI_API_KEY environment variable.
        /// </summary>
        /// <returns>An <see cref="AgentCredential"/> instance configured for Google Gemini.</returns>
        /// <exception cref="InvalidOperationException">Thrown when GOOGLE_GEMINI_API_KEY environment variable is not set.</exception>
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
