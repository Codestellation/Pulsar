using System;
using Codestellation.Pulsar.Misc;
using Codestellation.Pulsar.Triggers;
using NUnit.Framework;

namespace Codestellation.Pulsar.Tests.Triggers
{
    [TestFixture]
    public class WeeklyScheduleTests
    {
        [TestCase("2016-03-03 03:05:43Z", "2016-03-04 20:00:00Z", "Should be a day in the same week")]
        [TestCase("2016-03-04 03:05:43Z", "2016-03-04 20:00:00Z", "Should be the same day")]
        [TestCase("2016-03-05 03:05:43Z", "2016-03-11 20:00:00Z", "Should be a day at the next week")]
        [TestCase("2016-03-04 23:05:43Z", "2016-03-11 20:00:00Z", "Should be a day at the next week")]
        public void Should_calculate_next_occurrence(string nowString, string expectedString, string message)
        {
            //given
            var now = DateTime.Parse(nowString).ToUniversalTime();
            Clock.UtcNowFunction = () => now;
            var days = new[] { DayOfWeek.Friday };
            var times = new[] { TimeSpan.FromHours(20) };
            var schedule = new WeeklySchedule(days, times, TimeZoneInfo.Local);
            Console.WriteLine(schedule);

            //when
            var next = schedule.NextAt;

            //then
            var expected = DateTime.Parse(expectedString).ToUniversalTime();

            DateTime dateTime = next.Value;
            Assert.That(dateTime, Is.EqualTo(expected), message);
            Assert.That(dateTime.Kind, Is.EqualTo(DateTimeKind.Utc));
        }

        [TearDown]
        public void RepairClock()
        {
            Clock.UseDefault();
        }
    }
}