using System;
using Codestellation.Pulsar.Cron;
using NUnit.Framework;

namespace Codestellation.Pulsar.Tests.Cron
{
    [TestFixture]
    public class CronExpressionTests
    {
        [Test]
        public void Should_correctly_show_nearest_occurence()
        {
            var datetime = new DateTime(2012, 10, 15);

            var expression = new CronExpression("0 0 12 * * ?");

            var nearest = expression.NearestFrom(datetime);

            var expected = datetime.AddHours(12);

            Assert.That(nearest.Value, Is.EqualTo(expected));
        }
    }
}