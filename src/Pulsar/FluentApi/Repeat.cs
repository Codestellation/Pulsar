using System;

namespace Codestellation.Pulsar.FluentApi
{
    /// <summary>
    /// Contains few extension methods and properties to support fluent api
    /// </summary>
    public static class Repeat
    {
        /// <summary>
        /// Timespan that equals to a minute
        /// </summary>
        public static readonly TimeSpan Minutely = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Timespan that equals to a hour
        /// </summary>
        public static readonly TimeSpan Hour = TimeSpan.FromHours(1);

        /// <summary>
        /// Timespan that equals to a day
        /// </summary>
        public static readonly TimeSpan Daily = TimeSpan.FromDays(1);

        /// <summary>
        /// Timespan that equals to a second
        /// </summary>
        public static readonly TimeSpan EverySecond = TimeSpan.FromSeconds(1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static TimeSpan Every(TimeSpan timeSpan)
        {
            if (timeSpan <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(timeSpan), "Should be greater than TimeSpan.Zero");
            }
            return timeSpan;
        }
    }
}