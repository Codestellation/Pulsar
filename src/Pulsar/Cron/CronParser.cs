using System;
using System.Collections.Generic;
using System.Linq;

namespace Codestellation.Pulsar.Cron
{
    public static class CronParser
    {
        public static int ParseNumber(string token, ref int index, int min, int max)
        {
            if (char.IsDigit(token[index]))
            {
                var result = ParseDigit(token[index]);
                index++;
                if (index < token.Length && char.IsDigit(token[index]))
                {
                    result = result * 10 + ParseDigit(token[index]);
                    index++;
                }

                if (result < min || max < result)
                {
                    throw new FormatException();
                }
                return result;
            }

            throw new FormatException();
        }

        private static int ParseDigit(char value)
        {
            return Convert.ToInt32(value) - 48;
        }

        public static bool IsRange(string token)
        {
            return token.Contains(CronSymbols.Range.ToString());
        }

        public static IEnumerable<int> ParseRange(string token, int min, int max)
        {
            var values = new List<int>();
            int index = 0;

            int initial = ParseNumber(token, ref index, min, max);

            index++;
            var final = ParseNumber(token, ref index, min, max);
            values.AddRange(Enumerable.Range(initial, final - initial + 1));
            return values;
        }

        public static bool IsIncrement(string token)
        {
            return token.Contains(CronSymbols.Increment);
        }

        public static IEnumerable<int> ParseIncrement(string token, int min, int max)
        {
            var values = new List<int>();
            int index = 0;

            int initial = ParseNumber(token, ref index, min, max);

            index++;
            var increment = ParseNumber(token, ref index, min, max);

            for (int i = initial; i <= max; i += increment)
            {
                values.Add(i);
            }
            return values;
        }

        public static bool IsLastDayOfMonth(string token)
        {
            return CompareTokenIgnoreCase(token, CronSymbols.Last.ToString());
        }

        public static bool IsWeekday(string token)
        {
            return CompareTokenIgnoreCase(token, CronSymbols.Weekday.ToString());
        }

        public static bool IsLastWeekday(string token)
        {
            return CompareTokenIgnoreCase(token, CronSymbols.Last + CronSymbols.Weekday.ToString());
        }

        private static bool CompareTokenIgnoreCase(string candidate, string ethalon)
        {
            return candidate.Equals(ethalon, StringComparison.OrdinalIgnoreCase);
        }
    }
}