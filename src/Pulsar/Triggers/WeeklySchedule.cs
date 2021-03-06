﻿using System;
using System.Collections.Generic;
using Codestellation.Pulsar.Misc;

namespace Codestellation.Pulsar.Triggers
{
    /// <summary>
    /// Encapsulates logic
    /// </summary>
    public class WeeklySchedule : ISchedule
    {
        private readonly TimeZoneInfo _timeZone;
        private readonly SortedSet<DayOfWeek> _days;
        private readonly SortedSet<TimeSpan> _times;

        /// <summary>
        /// Creates new instance of <see cref="WeeklySchedule"/>
        /// </summary>
        /// <param name="days">Days of week to trigger an event on</param>
        /// <param name="times">Times of day to trigger an event on</param>
        /// <param name="timeZone">Time zone of a <see cref="WeeklySchedule"/></param>
        public WeeklySchedule(IEnumerable<DayOfWeek> days, IEnumerable<TimeSpan> times, TimeZoneInfo timeZone)
        {
            _timeZone = timeZone;
            if (days == null)
            {
                throw new ArgumentNullException(nameof(days));
            }
            if (times == null)
            {
                throw new ArgumentNullException(nameof(times));
            }

            if (timeZone == null)
            {
                throw new ArgumentNullException(nameof(timeZone));
            }

            _days = new SortedSet<DayOfWeek>(days);
            _times = new SortedSet<TimeSpan>(times);

            foreach (var time in _times)
            {
                if (TimeSpan.FromDays(1) <= time)
                {
                    throw new ArgumentOutOfRangeException(nameof(times), $"All element must be lower than {TimeSpan.FromDays(1)}, but was {time}");
                }
            }
        }

        /// <summary>
        /// Returns next datetime if possible
        /// <remarks>Should return <see cref="DateTimeKind.Utc"/> <see cref="DateTime"/></remarks>
        /// </summary>
        public DateTime? NextAt
        {
            get
            {
                var now = Clock.UtcNow;
                var day = now.DayOfWeek;
                var time = now.TimeOfDay;
                DateTime today = now.Date;

                if (_days.Contains(day))
                {
                    foreach (var candidate in _times)
                    {
                        if (time <= candidate)
                        {
                            return today.Add(candidate);
                        }
                    }
                }

                for (int offsetDays = 1; offsetDays < 8; offsetDays++)
                {
                    var nextDay = today.AddDays(offsetDays);
                    if (_days.Contains(nextDay.DayOfWeek))
                    {
                        return nextDay.Add(_times.Min);
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            var times = string.Join(",", _times);
            var days = string.Join(",", _days);
            return $"{times} {days} {_timeZone.StandardName}";
        }
    }
}