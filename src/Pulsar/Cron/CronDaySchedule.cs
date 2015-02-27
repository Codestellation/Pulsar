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

        public CronDaySchedule(SimpleCronField second, SimpleCronField minute, SimpleCronField hour)
        {
            _hourIndex = new IntegerIndex(hour);
            _minuteIndex = new IntegerIndex(minute);
            _secondIndex = new IntegerIndex(second);

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

        public TimeSpan MinTime
        {
            get { return new TimeSpan(_hour.MinValue, _second.MinValue, _minute.MinValue); }
        }

        public bool TryGetTimeAfter(TimeSpan timeOfDay, out TimeSpan fireAt)
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
                fireAt = TimeSpan.MinValue;
                return false;
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
                fireAt = TimeSpan.MinValue;
                return false;
            }

            fireAt = new TimeSpan(hour, minute, second);
            return true;

        }
    }
}