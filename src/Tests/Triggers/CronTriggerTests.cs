using System;
using Codestellation.Pulsar.Cron;
using Codestellation.Pulsar.Misc;
using Codestellation.Pulsar.Timers;
using Codestellation.Pulsar.Triggers;
using NUnit.Framework;

namespace Codestellation.Pulsar.Tests.Triggers
{
    [TestFixture]
    public class CronTriggerTests
    {
        [Test]
        public void Should_run_in_specified_time_zone()
        {
            //given
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Ekaterinburg Standard Time");
            var builder = new CronExpressionBuilder
            {
                Seconds = "0",
                Minutes = "0",
                Hours = "15",
                DayOfMonth = "10",
                Month = "*",
                DayOfWeek = "?"
            };
            var trigger = new CronTrigger(builder.ToCronExpression(), timeZone, new StubTimer());

            Clock.UtcNowFunction = () => new DateTime(2016, 4, 3, 3, 5, 43, DateTimeKind.Utc);

            //when
            var actual = trigger.NextFireAt.Value;

            //then
            var expected = new DateTime(2016, 4, 10, 10, 0, 0, DateTimeKind.Utc);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TearDown]
        public void RepairClock()
        {
            Clock.UseDefault();
        }
    }
}