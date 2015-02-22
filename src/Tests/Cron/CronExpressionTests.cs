using System;
using Codestellation.Pulsar.Cron;
using NUnit.Framework;

namespace Codestellation.Pulsar.Tests.Cron
{
    [TestFixture]
    public class CronExpressionTests
    {
        [TestCase(10, 15)] //before 12am - this day
        [TestCase(15, 16)] //after 12am - next day
        public void Should_correctly_show_nearest_dayly_occurence(int hour, int expecatedDay)
        {
            var datetime = new DateTime(2012, 10, 15).AddHours(hour);

            var expression = new CronExpression("0 0 12 * * ?");

            var nearest = expression.NearestFrom(datetime);

            var expected = new DateTime(2012, 10, expecatedDay, 12, 0, 0);

            Assert.That(nearest.Value, Is.EqualTo(expected));
        }
        
        [Test]
        public void Should_correctly_show_nearest_occurence2()
        {
            var datetime = new DateTime(2012, 1, 15);

            var expression = new CronExpression("0 0 12 L 3,6 ?");

            var nearest = expression.NearestFrom(datetime);

            var expected = new DateTime(2012,3,31,12,0,0);

            Assert.That(nearest.Value, Is.EqualTo(expected));
        }
    }
}