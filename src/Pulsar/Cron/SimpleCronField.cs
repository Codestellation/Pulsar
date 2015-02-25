using System;
using System.Collections.Generic;
using System.Linq;

namespace Codestellation.Pulsar.Cron
{
    /// <summary>
    /// Supports values, lists, ranges increment (, - * /)
    /// </summary>
    public class SimpleCronField
    {
        private readonly int _minValue;
        private readonly int _maxValue;
        private readonly SortedSet<int> _values;
        private readonly List<Func<DateTime, bool>> _selectors;

        protected SimpleCronField(string token, int minValue, int maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
            _values = new SortedSet<int>();
            _selectors = new List<Func<DateTime, bool>>();

            var subTokens = CronParser.Tokenize(token,CronSymbols.Comma);

            foreach (var subToken in subTokens)
            {
                ParseToken(subToken);
            }

            if (_values.Count > 0)
            {
                AddSelector(ContainsValue);
            }
        }

        public IEnumerable<int> Values
        {
            get { return _values; }
        }

        private bool ContainsValue(DateTime arg)
        {
            return _values.Contains((int)arg.DayOfWeek);
        }

        protected virtual void ParseToken(string token)
        {
            if (CronParser.IsAllVallues(token))
            {
                _values.AddRange(Enumerable.Range(_minValue, _maxValue));
                return;
            }
            if (CronParser.IsRange(token))
            {
                _values.AddRange(CronParser.ParseRange(token, _minValue, _maxValue));
                return;
            }
            if (CronParser.IsIncrement(token))
            {
                _values.AddRange(CronParser.ParseIncrement(token, _minValue, _maxValue));
                return;
            }
            if (CronParser.IsNumber(token))
            {
                var index = 0;
                _values.Add(CronParser.ParseNumber(token, ref index, _minValue, _maxValue));
                return;
            }

            var message = string.Format("Could not parse value '{0}'", token);
            throw new FormatException(message);
        }


        protected void AddSelector(Func<DateTime, bool> selector)
        {
            _selectors.Add(selector);
        }

        protected void ClearSelectors()
        {
            _selectors.Clear();
        }

        protected void ClearValues()
        {
            _values.Clear();
        }

        public bool ShouldFire(DateTime date)
        {
            return _selectors.Any(selector => selector(date));
        }

        public static SimpleCronField ParseSeconds(string second)
        {
            return new SimpleCronField(second, 0, 59);
        }

        public static SimpleCronField ParseMinutes(string minute)
        {
            return new SimpleCronField(minute, 0, 59);
        }

        public static SimpleCronField ParseHours(string hour)
        {
            return new SimpleCronField(hour, 0, 23);
        }

        public static SimpleCronField ParseMonth(string month)
        {
            return new SimpleCronField(month, 1, 12);
        }

        public static SimpleCronField ParseYear(string year)
        {
            return new SimpleCronField(year, 2000, 2100);
        }
    }
}