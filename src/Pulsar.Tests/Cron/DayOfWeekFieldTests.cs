using System;
using Codestellation.Pulsar.Cron;
using NUnit.Framework;

namespace Codestellation.Pulsar.Tests.Cron
{
    [TestFixture]
    public class DayOfWeekFieldTests
    {
        //, - * ? / L #
        [Test]
        public void Should_parse_simple_values()
        {
            var field = new DayOfWeekField("1-3,6/1");
            Assert.That(field.NotSpecified, Is.False);
            CollectionAssert.AreEqual(new[] { 1,2,3,6,7 }, field.Values);
        }

        [Test]
        public void Should_parse_not_specified()
        {
            var field = new DayOfWeekField("?");

            Assert.That(field.NotSpecified, Is.True);
        }
        
        [Test]
        public void Should_parse_simple_last_value()
        {
            var field = new DayOfWeekField("L");
            var date = new DateTime(2015, 2, 28);
            
            Assert.That(field.ShouldFire(date), Is.True);
            
            Assert.That(field.NotSpecified, Is.False);
        }
        
        [TestCase(23, true)]
        [TestCase(28, false)]
        public void Should_parse_simple_numbered_weekday(int day, bool expected)
        {
            var field = new DayOfWeekField("2#4"); //4th monday
            var date = new DateTime(2015, 2, day);

            Assert.That(field.ShouldFire(date), Is.EqualTo(expected));

            Assert.That(field.NotSpecified, Is.False);
        }
    }
}