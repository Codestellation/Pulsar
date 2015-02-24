using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Codestellation.Pulsar.Cron
{
    public class DayOfWeekField
    {
        private readonly SortedSet<int> _values;
        private readonly List<Func<DateTime, bool>> _selectors;


        public bool NotSpecified { get; private set; }

        public IEnumerable<int> Values
        {
            get { return _values; }
        }

        public DayOfWeekField(string token)
        {


            _values = new SortedSet<int>();
            _selectors = new List<Func<DateTime, bool>> { HasValue };

            if (CronParser.IsNotSpecifed(token))
            {
                NotSpecified = true;
                return;
            }

            var subTokens = token.Split(new []{CronSymbols.Comma}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var subToken in subTokens)
            {
                ParseToken(subToken);
            }
        }

        private bool HasValue(DateTime arg)
        {
            return _values.Contains((int)arg.DayOfWeek);
        }

        private void ParseToken(string token)
        {
            const int sunday = 1;
            const int saturday = 7;

            if (CronParser.IsRange(token))
            {
                _values.AddRange(CronParser.ParseRange(token, sunday, saturday));
                return;
            }
            if (CronParser.IsIncrement(token))
            {
                _values.AddRange(CronParser.ParseIncrement(token, sunday, saturday));
                return;
            }
            if (CronParser.IsNumberedWeekday(token))
            {
                _selectors.Add(CronParser.ParseNumberedWeekday(token));
                return;
            }
            if (CronParser.IsLast(token))
            {
                _selectors.Add(IsLastDay);
                return;
            }
            if (CronParser.IsAllVallues(token))
            {
                _values.AddRange(Enumerable.Range(sunday, saturday));
                return;
            }
            if(CronParser.IsNumber(token))
            {
                var index = 0;
                _values.Add(CronParser.ParseNumber(token, ref index, sunday, saturday));
                return;
            }

            var message = string.Format("Could not parse value '{0}'", token);
            throw new FormatException(message);
        }

        public bool ShouldFire(DateTime date)
        {
            return _selectors.Any(selector => selector(date));
        }

        private bool IsLastDay(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday;
        }
    }
}