using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core
{
    public static class Env
    {
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

        public static string? GetOptional(string key)
        {
            return
                Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process)
                ?? Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.User)
                ?? Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Machine);
        }
    }
}
