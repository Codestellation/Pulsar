using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Permissions;

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

            var dayOfWeek = ToDayOfWeek(int.Parse(subtokens[0]));
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
            var daysToAdd = (number-1)*7;
            var candidate = first.AddDays(daysToAdd);
            return candidate.Equals(date.Date);
        }

        public static string[] Tokenize(string token, char separator)
        {
            return token.Split(new[] {separator}, StringSplitOptions.RemoveEmptyEntries);
        }

        public static DayOfWeek ToDayOfWeek(int dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case 1:
                    return DayOfWeek.Sunday;
                case 2:
                    return DayOfWeek.Monday;
                case 3:
                    return DayOfWeek.Tuesday;
                case 4:
                    return DayOfWeek.Wednesday;
                case 5:
                    return DayOfWeek.Thursday;
                case 6: 
                    return DayOfWeek.Friday;
                case 7:
                    return DayOfWeek.Saturday;
            }
            var message = string.Format("Day of week should be digit between 1 and 7, but was {0}", dayOfWeek);
            throw new FormatException(message);
        }
    }
}