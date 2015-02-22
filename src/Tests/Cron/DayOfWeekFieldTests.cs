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
            var field = DayOfWeekField.Parse("1-3,6/1");
            Assert.That(field.NotSpecified, Is.False);
            CollectionAssert.AreEqual(new[] { 1,2,3,6,7 }, field.Values);
        }

        [Test]
        public void Should_parse_not_specified()
        {
            var field = DayOfWeekField.Parse("?");

            Assert.That(field.NotSpecified, Is.True);
        }
        
        [Test]
        public void Should_parse_simple_last_value()
        {
            var field = DayOfWeekField.Parse("L");
            var date = new DateTime(2015, 2, 28);
            
            Assert.That(field.ShouldFire(date), Is.True);
            
            Assert.That(field.NotSpecified, Is.False);
        }
    }
}