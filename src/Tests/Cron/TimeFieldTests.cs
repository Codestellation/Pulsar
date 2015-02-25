using System.Linq;
using Codestellation.Pulsar.Cron;
using NUnit.Framework;

namespace Codestellation.Pulsar.Tests.Cron
{
    [TestFixture]
    public class TimeFieldTests
    {
        ///
        [Test]
        public void Can_parse_simple_seconds()
        {
            var field = SimpleCronField.ParseSeconds("42");
            CollectionAssert.AreEqual(new[] { 42 }, field.Values);
        }

        [Test]
        public void Can_parse_comma_separated_values()
        {
            var field = SimpleCronField.ParseSeconds("5,42,12");
            CollectionAssert.AreEqual(new[] { 5, 12, 42 }, field.Values);
        }

        [Test]
        public void Can_parse_all_value()
        {
            var field = SimpleCronField.ParseSeconds("*");
            var expected = Enumerable.Range(0, 60);
            CollectionAssert.AreEqual(expected, field.Values);
        }

        [Test]
        public void Can_parse_ranges_value()
        {
            var field = SimpleCronField.ParseSeconds("1-3,20-22,42");

            CollectionAssert.AreEqual(new[] { 1, 2, 3, 20, 21, 22, 42 }, field.Values);
        }

        [Test]
        public void Can_parse_increment_values()
        {
            var field = SimpleCronField.ParseSeconds("0-3,20/5");

            CollectionAssert.AreEqual(new[] { 0, 1, 2, 3, 20, 25, 30, 35, 40, 45, 50, 55 }, field.Values);
        }
    }
}