using System;
using System.Collections.Generic;

namespace Codestellation.Pulsar.Cron
{
    public class CronDaySchedule
    {
        private readonly SimpleCronField _second;
        private readonly SimpleCronField _minute;
        private readonly SimpleCronField _hour;
        private IntegerIndex _hourIndex;
        private IntegerIndex _minuteIndex;
        private IntegerIndex _secondIndex;
        private readonly TimeSpan _timeSpan;

        public CronDaySchedule(SimpleCronField second, SimpleCronField minute, SimpleCronField hour)
        {
            _hourIndex = new IntegerIndex(hour);
            _minuteIndex = new IntegerIndex(minute);
            _secondIndex = new IntegerIndex(second);

            _second = second;
            _minute = minute;
            _hour = hour;
            _timeSpan = new TimeSpan(_hour.MinValue, _second.MinValue, _minute.MinValue);
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

        public TimeSpan MinTime => _timeSpan;

        public bool TryGetTimeAfter(TimeSpan timeOfDay, out TimeSpan fireAt)
        {
            while (true)
            {
                var hours = timeOfDay.Hours;

                var hour = _hourIndex.GetValue(hours);

                if (hour == IntegerIndex.NotFound)
                {
                    fireAt = TimeSpan.MinValue;
                    return false;
                }
                if (hour > hours)
                {
                    fireAt = new TimeSpan(hour, _minute.MinValue, _second.MinValue);
                    return true;
                }

                var minutes = timeOfDay.Minutes;
                var minute = _minuteIndex.GetValue(minutes);

                if (minute == IntegerIndex.NotFound)
                {
                    var nextHour = new TimeSpan(hours + 1, 0, 0);
                    timeOfDay = nextHour;
                    continue;
                }
                if (minute > minutes)
                {
                    fireAt = new TimeSpan(hour, minute, _second.MinValue);
                    return true;
                }

                var seconds = timeOfDay.Seconds;
                var second = _secondIndex.GetValue(seconds);

                if (second == IntegerIndex.NotFound)
                {
                    var nextMinute = new TimeSpan(hours, minute + 1, 0);
                    timeOfDay = nextMinute;
                    continue;
                }

                fireAt = new TimeSpan(hour, minute, second);
                return true;
            }
        }
    }
}