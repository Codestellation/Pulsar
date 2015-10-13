using System;
using Codestellation.Pulsar.Misc;
using Codestellation.Pulsar.Triggers;

namespace Codestellation.Pulsar.FluentApi
{
    /// <summary>
    /// Contains few extension methods and properties to support fluent api
    /// </summary>
    public static class Start
    {
        /// <summary>
        /// Use to start <see cref="SimpleTimerTrigger"/> immediately
        /// </summary>
        public static readonly DateTime Immediately = DateTime.MinValue.ToUniversalTime();

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

        /// <summary>
        /// Generates point in time with specified timespan from now
        /// </summary>
        /// <param name="timeSpan">Time to start timer after</param>
        public static DateTime After(TimeSpan timeSpan)
        {
            if (timeSpan <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(timeSpan), "Should be greater than TimeSpan.Zero");
            }

            return Clock.UtcNow + timeSpan;
        }
    }
}