using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Codestellation.Pulsar.Cron
{
    public class DayOfWeekField
    {
        private readonly bool _lastDay;
        public IEnumerable Values { get; private set; }
        public bool NotSpecified { get; private set; }

        private DayOfWeekField(bool notSpecified = false, bool lastDay = false)
        {
            _lastDay = lastDay;
            NotSpecified = notSpecified;
        }

        private DayOfWeekField(IEnumerable<int> values)
        {
            Values = values.ToList();
        }

        public static DayOfWeekField Parse(string token)
        {
            const int sunday = 1;
            const int saturday = 7;

            if (CronParser.IsNotSpecifed(token))
            {
                return new DayOfWeekField(notSpecified:true);
            }

            if (CronParser.IsLast(token))
            {
                return new DayOfWeekField(lastDay:true);
            }

            if (CronParser.IsAllVallues(token))
            {
                var allValues = new List<int>(saturday);
                allValues.AddRange(Enumerable.Range(sunday, saturday));
                return new DayOfWeekField(allValues);
            }

            var subTokens = token.Split(CronSymbols.Comma);
            var values = new SortedSet<int>();

            foreach (var subToken in subTokens)
            {
                var subValues = ParseToken(subToken, sunday, saturday);
                foreach (var value in subValues)
                {
                    values.Add(value);
                }
            }

            return new DayOfWeekField(values);
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

        public bool ShouldFire(DateTime date)
        {
            if (date.Day == CronDateHelper.GetLastDayOfMonth(date))
            {
                return true;
            }
            return false;
        }
    }
}