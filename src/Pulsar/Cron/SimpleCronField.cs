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
        private readonly CronFieldSettings _settings;
        private readonly List<int> _values;
        private readonly List<Func<DateTime, bool>> _selectors;

        protected SimpleCronField(string token, CronFieldSettings settings)
        {
            _settings = settings;
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

        private bool ContainsValue(DateTime date)
        {
            return _values.Contains(_settings.DatePartSelector(date));
        }

        protected virtual void ParseToken(string token)
        {
            if (CronParser.IsAllVallues(token))
            {
                _values.AddRange(CronParser.Range(_settings.MinValue, _settings.MaxValue));
                return;
            }
            if (CronParser.IsRange(token))
            {
                _values.AddRange(CronParser.ParseRange(token, _settings.MinValue, _settings.MaxValue));
                return;
            }
            if (CronParser.IsIncrement(token))
            {
                _values.AddRange(CronParser.ParseIncrement(token, _settings.MinValue, _settings.MaxValue));
                return;
            }
            if (CronParser.IsNumber(token))
            {
                var index = 0;
                _values.Add(CronParser.ParseNumber(token, ref index, _settings.MinValue, _settings.MaxValue));
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