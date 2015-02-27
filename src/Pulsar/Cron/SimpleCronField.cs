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
        public readonly CronFieldSettings Settings;
        private readonly List<int> _values;
        private readonly List<Func<DateTime, bool>> _selectors;

        protected SimpleCronField(string token, CronFieldSettings settings)
        {
            Settings = settings;
            _values = new List<int>();
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
            var values = _values.Distinct().ToArray();

            _values.Clear();
            _values.AddRange(values);
            _values.Sort();
        }

        public IEnumerable<int> Values
        {
            get { return _values; }
        }

        public int MinValue
        {
            get { return _values[0]; }
        }

        public int MaxValue
        {
            get { return _values[_values.Count - 1]; }
        }

        private bool ContainsValue(DateTime date)
        {
            return _values.Contains(Settings.DatePartSelector(date));
        }

        protected virtual void ParseToken(string token)
        {
            if (CronParser.IsAllVallues(token))
            {
                _values.AddRange(CronParser.Range(Settings.MinValue, Settings.MaxValue));
                return;
            }
            if (CronParser.IsRange(token))
            {
                _values.AddRange(CronParser.ParseRange(token, Settings.MinValue, Settings.MaxValue));
                return;
            }
            if (CronParser.IsIncrement(token))
            {
                _values.AddRange(CronParser.ParseIncrement(token, Settings.MinValue, Settings.MaxValue));
                return;
            }
            if (CronParser.IsNumber(token))
            {
                var index = 0;
                _values.Add(CronParser.ParseNumber(token, ref index, Settings.MinValue, Settings.MaxValue));
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
            return new SimpleCronField(second, CronFieldSettings.Second);
        }

        public static SimpleCronField ParseMinutes(string minute)
        {
            return new SimpleCronField(minute, CronFieldSettings.Minute);
        }

        public static SimpleCronField ParseHours(string hour)
        {
            return new SimpleCronField(hour, CronFieldSettings.Hour);
        }

        public static SimpleCronField ParseMonth(string month)
        {
            return new SimpleCronField(month, CronFieldSettings.Month);
        }

        public static SimpleCronField ParseYear(string year)
        {
            return new SimpleCronField(year, CronFieldSettings.Year);
        }
    }
}