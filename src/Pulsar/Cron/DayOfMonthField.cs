using System;
using System.Collections.Generic;
using System.Linq;

namespace Codestellation.Pulsar.Cron
{
    public class DayOfMonthField
    {
        private readonly DayOfMonthChecker _selector;

        private delegate bool DayOfMonthChecker(DayOfMonthField field, DateTime point);

        private readonly List<int> _values;

        private DayOfMonthField(DayOfMonthChecker selector)
        {
            _selector = selector;
        }

        private DayOfMonthField(IEnumerable<int> allValues)
        {
            _selector = FromList;

            _values = new List<int>(allValues);
        }

        private DayOfMonthField()
        {
            NotSpecified = true;
            _selector = delegate { return false; };
        }

        public bool NotSpecified { get; private set; }

        public static DayOfMonthField Parse(string dayOfMonth)
        {
            const int min = 1;
            const int max = 31;

            if (CronParser.IsNotSpecifed(dayOfMonth))
            {
                return new DayOfMonthField();
            }
            if (CronParser.IsAllVallues(dayOfMonth))
            {
                var allValues = new List<int>(max);
                allValues.AddRange(Enumerable.Range(min, max));
                return new DayOfMonthField(allValues);
            }

            if (CronParser.IsLast(dayOfMonth))
            {
                return new DayOfMonthField(LastDayOfMonth);
            }
            else if (CronParser.IsWeekday(dayOfMonth))
            {
                return new DayOfMonthField(Weekday);
            }
            else if (CronParser.IsLastWeekday(dayOfMonth))
            {
                return new DayOfMonthField(LastWeekday);
            }


            var subTokens = dayOfMonth.Split(CronSymbols.Comma);
            var values = new SortedSet<int>();

            foreach (var token in subTokens)
            {
                var subValues = ParseToken(token, min, max);
                foreach (var value in subValues)
                {
                    values.Add(value);
                }
            }

            return new DayOfMonthField(values);
        }

        private static bool FromList(DayOfMonthField field, DateTime point)
        {
            return field._values.Contains(point.Day);
        }

        private static bool Weekday(DayOfMonthField field, DateTime point)
        {
            switch (point.DayOfWeek)
            {
                default:
                    return true;
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    return false;
            }
        }

        private static bool LastDayOfMonth(DayOfMonthField field, DateTime point)
        {
            return point.Day ==  CronDateHelper.GetLastDayOfMonth(point);
        }

        private static bool LastWeekday(DayOfMonthField field, DateTime point)
        {
            var lastDay = CronDateHelper.GetLastDayOfMonth(point.Year, point.Month);
            var candidate = new DateTime(point.Year, point.Month, lastDay);

            while (!Weekday(field, candidate))
            {
                candidate = candidate.AddDays(-1);
            }

            return point.Day == candidate.Day;
        }

        private static IEnumerable<int> ParseToken(string token, int min, int max)
        {
            if (CronParser.IsRange(token))
            {
                return CronParser.ParseRange(token, min, max);
            }
            else if (CronParser.IsIncrement(token))
            {
                return CronParser.ParseIncrement(token, min, max);
            }
            else
            {
                var index = 0;
                return new[] { CronParser.ParseNumber(token, ref index, min, max) };
            }
        }

        public IEnumerable<int> GetValues(int year, int month)
        {
            var maxValue = CronDateHelper.GetLastDayOfMonth(year, month);
            return _values.Where(x => x <= maxValue).ToArray();
        }


        public bool ShouldFire(DateTime point)
        {
            return _selector(this, point);
        }
    }
}