using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core
{
    /// <summary>
    /// Provides utility methods for accessing environment variables.
    /// </summary>
    public static class Env
    {
        /// <summary>
        /// Gets a required environment variable value.
        /// </summary>
        /// <param name="key">The name of the environment variable to retrieve.</param>
        /// <returns>The trimmed value of the environment variable.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the environment variable is not set or is whitespace.</exception>
        /// <remarks>
        /// Searches for the environment variable in the following order:
        /// <list type="number">
        /// <item>Process-level variables</item>
        /// <item>User-level variables</item>
        /// <item>Machine-level variables</item>
        /// </list>
        /// </remarks>
        public static string GetRequired(string key)
        {
            var value =
                Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process)
                ?? Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.User)
                ?? Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Machine);

            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidOperationException($"Environment variable '{key}' is not set.");

            return value.Trim();
        }

        /// <summary>
        /// Gets an optional environment variable value.
        /// </summary>
        /// <param name="key">The name of the environment variable to retrieve.</param>
        /// <returns>The value of the environment variable, or <c>null</c> if not found.</returns>
        /// <remarks>
        /// Searches for the environment variable in the following order:
        /// <list type="number">
        /// <item>Process-level variables</item>
        /// <item>User-level variables</item>
        /// <item>Machine-level variables</item>
        /// </list>
        /// </remarks>
        public static string? GetOptional(string key)
        {
            return
                Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process)
                ?? Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.User)
                ?? Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Machine);
        }
    }
}
