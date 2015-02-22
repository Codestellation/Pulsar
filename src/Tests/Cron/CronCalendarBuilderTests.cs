using System;
using Codestellation.Pulsar.Cron;
using NUnit.Framework;

namespace Codestellation.Pulsar.Tests.Cron
{
    [TestFixture]
    public class CronCalendarBuilderTests
    {
        [Test]
        public void Can_build_calendar()
        {
            var builder = new CronCalendarBuilder()
                .SetMonth(TimeField.ParseMonth("1,6"))
                .SetDayOfMonth(DayOfMonthField.Parse("1,15"));

            var calendar = builder.BuildFor(2015);


            var begin = new DateTime(2015, 1, 1);
            var expected = new[] {begin, begin.AddDays(14), begin.AddMonths(5), begin.AddMonths(5).AddDays(14)};

            CollectionAssert.AreEqual(expected, calendar.ScheduledDays);
        }
    }
}