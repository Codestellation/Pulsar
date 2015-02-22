using System;
using System.Collections.Generic;
using System.Linq;

namespace Codestellation.Pulsar.Cron
{
    public class DayOfMonthField : ICronField
    {
        private readonly NearestSelector _selector;

        private delegate int NearestSelector(DayOfMonthField field, DateTime point);

        private readonly List<int> _values;

        private DayOfMonthField(NearestSelector selector)
        {
            _selector = selector;
        }

        private DayOfMonthField(IEnumerable<int> allValues)
        {
            _selector = FromList;

            _values = new List<int>(allValues);
        }

        public static DayOfMonthField Parse(string dayOfMonth)
        {
            const int min = 1;
            const int max = 31;

            if (dayOfMonth[0] == CronSymbols.AllValues)
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

        private static int FromList(DayOfMonthField field, DateTime point)
        {
            return field._values.Find(x => x >= point.Day);
        }

        private static int Weekday(DayOfMonthField field, DateTime point)
        {
            var day = point;
            while (day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday)
            {
                day = day.AddDays(1);
            }
            if (day.Month != point.Month)
            {
                return -1;
            }

            return day.Day;
        }

        private static int LastDayOfMonth(DayOfMonthField field, DateTime point)
        {
            return CronDateHelper.GetLastDayOfMonth(point.Year, point.Month);
        }

        private static int LastWeekday(DayOfMonthField field, DateTime point)
        {
            var lastDay = CronDateHelper.GetLastDayOfMonth(point.Year, point.Month);
            var day = new DateTime(point.Year, point.Month, lastDay);
            
            while (day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday)
            {
                day = day.AddDays(-1);
            }
            if (day.Day < point.Day)
            {
                return -1;
            }
            return day.Day;
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

        public int GetClosestTo(DateTime point)
        {
            return _selector(this, point);
        }
    }
}