using System;
using System.Collections.Generic;
using System.Linq;

namespace Codestellation.Pulsar.Cron
{
    public class DayOfMonthField
    {
        private int[] _values;

        private DayOfMonthField(IEnumerable<int> allValues)
        {
            _values = allValues.ToArray();
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
            int maxValue = 0;
            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    maxValue = 31;
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    maxValue = 30;
                    break;
                case 2:
                    maxValue = DateTime.IsLeapYear(year) ? 29 : 28;
                    break;
            }

            return _values.Where(x => x <= maxValue).ToArray();
        }
    }
}