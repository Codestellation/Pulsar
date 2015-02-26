using System;
using System.Collections.Generic;

namespace Codestellation.Pulsar.Cron
{
    public class CronDaySchedule
    {
        private readonly SimpleCronField _second;
        private readonly SimpleCronField _minute;
        private readonly SimpleCronField _hour;

        public CronDaySchedule(SimpleCronField second, SimpleCronField minute, SimpleCronField hour)
        {
            _second = second;
            _minute = minute;
            _hour = hour;
        }

        public IEnumerable<TimeSpan> Values
        {
            get
            {
                foreach (var hour in _hour.Values)
                {
                    foreach (var minute in _minute.Values)
                    {
                        foreach (var second in _second.Values)
                        {
                            yield return new TimeSpan(hour, minute, second);
                        }
                    }
                }
            } 
        }

        public IEnumerable<TimeSpan> TimeAfter(TimeSpan timeOfDay)
        {
            var hours = timeOfDay.Hours;
            var minutes = timeOfDay.Minutes;
            var seconds = timeOfDay.Seconds;

            foreach (var hour in _hour.Values)
            {
                if (hour < hours)
                {
                    continue;
                }
                var sameHour = hour == timeOfDay.Hours;

                foreach (var minute in _minute.Values)
                {
                    if (sameHour && minute < minutes)
                    {
                        continue;
                    }
                    var sameMinute = sameHour && minute == minutes;
                    foreach (var second in _second.Values)
                    {
                        if (sameMinute && second < seconds)
                        {
                            continue;
                        }
                        yield return new TimeSpan(hour, minute, second);
                    }
                }
            }
        }
    }
}