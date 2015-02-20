using System.Collections.Generic;
using System.Linq;

namespace Codestellation.Pulsar.Cron
{
    public class TimeField
    {
        private readonly List<int> _values;

        private TimeField(IEnumerable<int> values)
        {
            _values = new List<int>(values);
        }

        public static TimeField ParseSeconds(string seconds)
        {
            return Parse(seconds, 0, 59);
        }
        
        public static TimeField ParseMinutes(string minutes)
        {
            return Parse(minutes, 0, 59);
        }

        public static TimeField ParseHours(string hours)
        {
            return Parse(hours, 0, 23);
        }


        private static TimeField Parse(string timeToken, int min, int max)
        {
            if (timeToken[0] == CronSymbols.AllValues)
            {
                var allValues = new List<int>(max);
                allValues.AddRange(Enumerable.Range(min, max));
                return new TimeField(allValues);
            }

            var subTokens = timeToken.Split(CronSymbols.Comma);
            var values = new SortedSet<int>();

            foreach (var token in subTokens)
            {
                var subValues = ParseToken(token, min, max);
                foreach (var value in subValues)
                {
                    values.Add(value);
                }
            }

            return new TimeField(values);
        }

        public IEnumerable<int> Values
        {
            get { return _values; }
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

        public int Nearest(int hour)
        {
            return _values.Find(x => hour <= x);
        }
    }
}