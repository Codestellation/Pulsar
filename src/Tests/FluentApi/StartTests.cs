using System;
using Codestellation.Pulsar.FluentApi;
using Codestellation.Pulsar.Misc;
using NUnit.Framework;

namespace Codestellation.Pulsar.Tests.FluentApi
{
    [TestFixture]
    public class StartTests
    {
        [Test]
        public void Should_return_correct_time_in_start_after()
        {
            Clock.UtcNowFunction = () => new DateTime(2012, 10, 30, 12, 32, 01, DateTimeKind.Utc);
            TimeSpan minute = TimeSpan.FromMinutes(1);
            var startAt = Start.After(minute);

            Assert.That(startAt, Is.EqualTo(new DateTime(2012, 10, 30, 12, 33, 01, DateTimeKind.Utc)));

            Assert.That(startAt.Kind, Is.EqualTo(DateTimeKind.Utc));
        }

        [TearDown]
        public void RepairClock()
        {
            Clock.UseDefault();
        }
    }
}