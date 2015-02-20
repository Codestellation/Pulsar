using System.Collections.Generic;
using System.Linq;

namespace Codestellation.Pulsar.Cron
{
    public class TimeField
    {
        private readonly IEnumerable<int> _values;

        private TimeField(IEnumerable<int> values)
        {
            _values = values;
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
            var values = new List<int>();
            int index = 0;

            int initial = CronParser.ParseNumber(token, ref index, min, max);

            var theOnlyValue = token.Length == index;
            if (theOnlyValue)
            {
                values.Add(initial);
            }

            var hasRange = index < token.Length && token[index] == CronSymbols.Range;
            
            if (hasRange)
            {
                index++;
                var final = CronParser.ParseNumber(token, ref index, min, max);
                values.AddRange(Enumerable.Range(initial, final-initial+1));
            }

            var hasIncrement = index < token.Length && token[index] == CronSymbols.Increment;
            if (hasIncrement)
            {
                index++;
                var increment = CronParser.ParseNumber(token, ref index, min, max);

                for (int i = initial; i <= max; i += increment)
                {
                    values.Add(i);
                }
            }

            return values;
        }

        
    }
}