using System;
using Codestellation.Pulsar.FluentApi;
using Codestellation.Pulsar.Misc;
using NUnit.Framework;

namespace Codestellation.Pulsar.Tests.FluentApi
{
    [TestFixture]
    public class StartTests
    {
        private DateTime _clockSetAt;

        [SetUp]
        public void FreezeClock()
        {
            Clock.UtcNowFunction = () =>
            {
                _clockSetAt = new DateTime(2012, 10, 30, 12, 32, 01, DateTimeKind.Utc);
                return _clockSetAt;
            };
        }

        [Test]
        public void Should_return_correct_time_in_start_after()
        {
            TimeSpan minute = TimeSpan.FromMinutes(1);
            var startAt = Start.After(minute);

            Assert.That(startAt, Is.EqualTo(_clockSetAt.AddMinutes(1)));

            Assert.That(startAt.Kind, Is.EqualTo(DateTimeKind.Utc));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Should_return_correct_time_in_start_after_timespan_is_not_greater_than_zero(int minutes)
        {
            TimeSpan minute = TimeSpan.FromMinutes(minutes);
            var startAt = Start.After(minute);

            Assert.That(startAt, Is.EqualTo(_clockSetAt));
            Assert.That(startAt.Kind, Is.EqualTo(DateTimeKind.Utc));
        }

        [TearDown]
        public void RepairClock()
        {
            Clock.UseDefault();
        }
    }
}