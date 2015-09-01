using System;

namespace Codestellation.Pulsar.FluentApi
{
    /// <summary>
    /// Contains few extension methods and properties to support fluent api
    /// </summary>
    public static class Repeat
    {
        /// <summary>
        /// Contains few extension methods and properties to support fluent api
        /// </summary>
        public static class Every
        {
            /// <summary>
            /// Timespan that equals to a second
            /// </summary>
            public static readonly TimeSpan Second = TimeSpan.FromSeconds(1);

            /// <summary>
            /// Timespan that equals to a minute
            /// </summary>
            public static readonly TimeSpan Minute = TimeSpan.FromMinutes(1);

            /// <summary>
            /// Timespan that equals to a hour
            /// </summary>
            public static readonly TimeSpan Hour = TimeSpan.FromHours(1);

            /// <summary>
            /// Timespan that equals to a dat
            /// </summary>
            public static readonly TimeSpan Day = TimeSpan.FromDays(1);
        }
    }
}