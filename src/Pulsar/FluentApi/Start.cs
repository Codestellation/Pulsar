using System;
using Codestellation.Pulsar.Misc;
using Codestellation.Pulsar.Triggers;

namespace Codestellation.Pulsar.FluentApi
{
    /// <summary>
    /// Contains few extension methods and properties to support fluent api
    /// </summary>
    public class Start
    {
        /// <summary>
        /// Use to start <see cref="SimpleTimerTrigger"/> immediately
        /// </summary>
        public static DateTime Immediately = DateTime.MinValue.ToUniversalTime();

        /// <summary>
        /// Generates expresson
        /// </summary>
        /// <param name="timeOfDay">Should be valid time of day value</param>
        /// <param name="dateTimeKind"></param>
        /// <returns></returns>
        public static DateTime At(TimeSpan timeOfDay, DateTimeKind dateTimeKind)
        {
            if (timeOfDay < TimeSpan.FromSeconds(0) || TimeSpan.FromDays(1) < timeOfDay)
            {
                throw new ArgumentOutOfRangeException(nameof(timeOfDay), "Time of day should be in range from 0 up to 24 hours");
            }
            if (dateTimeKind == DateTimeKind.Unspecified)
            {
                throw new ArgumentOutOfRangeException(nameof(dateTimeKind), "DateTimeKind should be UTC or Local");
            }

            var currentDateTime = Clock.UtcNow;
            if (dateTimeKind == DateTimeKind.Local)
            {
                currentDateTime = currentDateTime.ToLocalTime();
            }

            var result = currentDateTime.Date.Add(timeOfDay);

            //Missed in current day. Plan it tomorrow
            if (currentDateTime.TimeOfDay > timeOfDay)
            {
                result = result.AddDays(1);
            }

            return result;
        }
    }
}