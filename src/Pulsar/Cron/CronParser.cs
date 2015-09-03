using System;
using System.Collections.Generic;
using System.Linq;

namespace Codestellation.Pulsar.Cron
{
    public static class CronParser
    {
        public static bool IsNumber(string token)
        {
            return token.All(char.IsDigit);
        }

        public static int ParseNumber(string token, ref int index, int min, int max)
        {
            int result = 0;
            do
            {
                result *= 10;
                result += ParseDigit(token[index]);
                index++;
            } while (index < token.Length && char.IsDigit(token[index]));

            if (result < min || max < result)
            {
                throw new FormatException();
            }
            return result;
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
            values.AddRange(Range(initial, final));
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

        public static bool IsLast(string token)
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

        public static bool IsAllVallues(string token)
        {
            return token.Contains(CronSymbols.AllValues);
        }

        public static bool IsNotSpecifed(string token)
        {
            if (token.Equals(CronSymbols.NotSpecified.ToString(), StringComparison.Ordinal))
            {
                return true;
            }
            if (token.Contains(CronSymbols.NotSpecified))
            {
                var message = string.Format("Not specified value '?' must be the only value, but was '{0}'", token);
                throw new FormatException(message);
            }
            return false;
        }

        public static bool IsNumberedWeekday(string token)
        {
            return token.Contains(CronSymbols.WeekdayNumber);
        }

        public static Func<DateTime, bool> ParseNumberedWeekday(string token)
        {
            var subtokens = Tokenize(token, CronSymbols.WeekdayNumber);

            var dayOfWeek = CronDateHelper.ToDayOfWeek(int.Parse(subtokens[0]));
            var numberOfDay = int.Parse(subtokens[1]);
            return date => IsNumberedWeekday(date, dayOfWeek, numberOfDay);
        }

        private static bool IsNumberedWeekday(DateTime date, DayOfWeek dayOfWeek, int number)
        {
            if (date.DayOfWeek != dayOfWeek)
            {
                return false;
            }

            var first = CronDateHelper.First(dayOfWeek, date);
            var daysToAdd = (number - 1) * 7;
            var candidate = first.AddDays(daysToAdd);
            return candidate.Equals(date.Date);
        }

        public static string[] Tokenize(string token, char separator)
        {
            return token.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static IEnumerable<int> Range(int start, int end)
        {
            for (int range = start; range <= end; range++)
            {
                yield return range;
            }
        }
    }
}