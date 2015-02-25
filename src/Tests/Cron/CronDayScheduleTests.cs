using System;
using System.Configuration;
using System.Linq;
using Codestellation.Pulsar.Cron;
using NUnit.Framework;

namespace Codestellation.Pulsar.Tests.Cron
{
    [TestFixture]
    public class CronDayScheduleTests
    {
        [Test]
        public void Should_enumerate_intraday_points()
        {
            var second = SimpleCronField.ParseSeconds("0/10");
            var minute = SimpleCronField.ParseMinutes("0");
            var hour = SimpleCronField.ParseHours("10");
            var schedule = new CronDaySchedule(second, minute, hour);

            var actual = schedule.Values.ToArray();

            var initial = new TimeSpan(10, 0, 0);
            var expected = new[]
            {
                initial.Add(TimeSpan.FromSeconds(00)),
                initial.Add(TimeSpan.FromSeconds(10)),
                initial.Add(TimeSpan.FromSeconds(20)),
                initial.Add(TimeSpan.FromSeconds(30)),
                initial.Add(TimeSpan.FromSeconds(40)),
                initial.Add(TimeSpan.FromSeconds(50)),
            };
            //note: order of values does matter!
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}