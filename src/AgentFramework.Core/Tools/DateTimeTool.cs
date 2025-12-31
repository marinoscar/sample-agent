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
        [Description("Gets the current date and time.")]
        public DateTime GetCurrentDateTime()
        {
            var timeZoneInfo = GetTimeZoneInfoFromEnvVariable();
            var utcNow = DateTime.UtcNow;
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, timeZoneInfo);
            return localTime;
        }

        [Description("Gets the current date (without time component).")]
        public DateOnly GetCurrentDate()
        {
            var timeZoneInfo = GetTimeZoneInfoFromEnvVariable();
            var utcNow = DateTime.UtcNow;
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, timeZoneInfo);
            return DateOnly.FromDateTime(localTime);
        }

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
