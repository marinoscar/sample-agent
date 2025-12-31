using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Tools
{
    /// <summary>
    /// Provides date and time related functionalities.
    /// </summary>
    public class DateTimeTool
    {

        /// <summary>
        /// Gets the current date and time in the configured time zone.
        /// </summary>
        /// <remarks>The time zone is determined by an environment variable. If the environment variable
        /// is not set or specifies an invalid time zone, the method may throw an exception. The returned value reflects
        /// the system's current time at the moment the method is called.</remarks>
        /// <returns>A <see cref="DateTime"/> value representing the current local date and time based on the configured time
        /// zone.</returns>
        [Description("Gets the current date and time.")]
        public DateTime GetCurrentDateTime()
        {
            var timeZoneInfo = GetTimeZoneInfoFromEnvVariable();
            var utcNow = DateTime.UtcNow;
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, timeZoneInfo);
            return localTime;
        }

        /// <summary>
        /// Gets the current date (without time component) in the configured time zone.
        /// </summary>
        /// <returns>A <see cref="DateOnly"/> value representing the current local date based on the configured time zone.</returns>
        [Description("Gets the current date (without time component).")]
        public DateOnly GetCurrentDate()
        {
            var timeZoneInfo = GetTimeZoneInfoFromEnvVariable();
            var utcNow = DateTime.UtcNow;
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, timeZoneInfo);
            return DateOnly.FromDateTime(localTime);
        }

        /// <summary>
        /// Creates an AI tool representation of the current date and time provider function.
        /// </summary>
        /// <remarks>Use the returned <see cref="AITool"/> to integrate the date and time provider as an
        /// AI tool within compatible frameworks or workflows.</remarks>
        /// <returns>An <see cref="AITool"/> instance that encapsulates the current date and time provider function.</returns>
        public AITool AsAITool()
        {
            return AIFunctionFactory.Create(GetCurrentDateTime);
        }

        private TimeZoneInfo GetTimeZoneInfoFromEnvVariable()
        {
            var envVarName = "AGENT_TIMEZONE_ID";
            var timeZoneId = Environment.GetEnvironmentVariable(envVarName);
            if (string.IsNullOrEmpty(timeZoneId))
            {
                return TimeZoneInfo.Local;
            }
            try
            {
                var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                return timeZoneInfo;
            }
            catch
            {
                return TimeZoneInfo.Local;
            }
        }
    }
}
