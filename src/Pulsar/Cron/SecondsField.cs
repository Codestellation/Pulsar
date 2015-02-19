using System;
using System.Collections.Generic;
using System.Linq;

namespace Codestellation.Pulsar.Cron
{
    public class SecondsField
    {
        private readonly IEnumerable<int> _values;
        private static readonly List<int> AllValues;

        static SecondsField()
        {
            AllValues = new List<int>(60);
            AllValues.AddRange(Enumerable.Range(0, 59));
        }

        private SecondsField(IEnumerable<int> values)
        {
            _values = values;
        }

        public IEnumerable<int> Values
        {
            get { return _values; }
        }

        public static SecondsField Parse(string s)
        {
            if (s[0] == CronSymbols.AllValues)
            {
                return new SecondsField(AllValues);
            }

            var subTokens = s.Split(CronSymbols.Comma);
            var values = new SortedSet<int>();

            foreach (var token in subTokens)
            {
                var subValues = ParseToken(token);
                foreach (var value in subValues)
                {
                    values.Add(value);
                }
            }

            return new SecondsField(values);
        }

        private static IEnumerable<int> ParseToken(string token)
        {
            var values = new List<int>();
            int index = 0;

            int initial = ParseNumber(token, ref index);

            var theOnlyValue = token.Length == index;
            if (theOnlyValue)
            {
                values.Add(initial);
            }

            if (index < token.Length && token[index] == CronSymbols.Range)
            {
                index++;
                var final = ParseNumber(token, ref index);

                values.AddRange(Enumerable.Range(initial, final-initial+1));
            }
            
            if (index < token.Length && token[index] == CronSymbols.Increment)
            {
                index++;
                var increment = ParseNumber(token, ref index);

                for (int i = initial; i <= 59; i += increment)
                {
                    values.Add(i);
                }
            }

            return values;
        }

        private static int ParseNumber(string token, ref int index)
        {
            if (char.IsDigit(token[index]))
            {
                var result = ParseDigit(token[index]);
                index++;
                if (index < token.Length && char.IsDigit(token[index]))
                {
                    result = result*10 + ParseDigit(token[index]);
                    index++;
                }

                if (result < 0 || 59 < result)
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
    }
}