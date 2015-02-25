using System;
using System.Linq;
using Codestellation.Pulsar.Cron;
using NUnit.Framework;

namespace Codestellation.Pulsar.Tests.Cron
{
    [TestFixture]
    public class DayOfMonthFieldTests
    {
        [Test]
        public void Can_parse_single_all_values()
        {
            var field = new DayOfMonthField("*");

            var values = field.Values.ToArray();

            var min = values.Min();
            var max = values.Max();

            Assert.That(values.Length, Is.EqualTo(31));
            Assert.That(min, Is.EqualTo(1));
            Assert.That(max, Is.EqualTo(31));
        }

        //, - * ? / L W
        [Test]
        public void Can_parse_list_of_value()
        {
            var field = new DayOfMonthField("10,20");

            var values = field.Values;

            var expected = new[] { 10, 20 };

            CollectionAssert.AreEqual(expected, values);
        }

        [Test]
        public void Can_parse_ranges()
        {
            var field = new DayOfMonthField("10,20-22");

            var values = field.Values;

            var expected = new[] { 10, 20, 21, 22 };

            CollectionAssert.AreEqual(expected, values);
        }

        [Test]
        public void Can_parse_ranges_increment()
        {
            var field = new DayOfMonthField("10,20-22,20/5");

            var values = field.Values;

            var expected = new[] { 10, 20, 21, 22, 25, 30 };

            CollectionAssert.AreEqual(expected, values);
        }

        [TestCase(1, 31)]
        [TestCase(2, 29)]
        [TestCase(3, 31)]
        [TestCase(4, 30)]
        [TestCase(5, 31)]
        [TestCase(6, 30)]
        [TestCase(7, 31)]
        [TestCase(8, 31)]
        [TestCase(9, 30)]
        [TestCase(10, 31)]
        [TestCase(11, 30)]
        [TestCase(12, 31)]
        public void Can_parse_last_day(int month, int lastDay)
        {
            var date = new DateTime(2012, month, lastDay);

            var field = new DayOfMonthField("L");

            Assert.That(field.ShouldFire(date), Is.True);
        }

        [TestCase(1, false)]
        [TestCase(7, false)]
        [TestCase(10, true)]
        [TestCase(28, false)]
        public void Can_parse_week_day(int day, bool expected)
        {
            var date = new DateTime(2015, 2, day);

            var field = new DayOfMonthField("W");

            Assert.That(field.ShouldFire(date), Is.EqualTo(expected));
        }

        [TestCase(27, true)]
        [TestCase(28, false)]
        public void Can_parse_last_week_day(int day, bool expected)
        {
            var date = new DateTime(2015, 2, day);

            var field = new DayOfMonthField("LW");

            Assert.That(field.ShouldFire(date), Is.EqualTo(expected));
        }

        [Test]
        public void Can_parse_not_specified()
        {
            var date = new DateTime(2015, 2, 3);

            var field = new DayOfMonthField("?");

            var shouldFire = field.ShouldFire(date);

            Assert.That(shouldFire, Is.False);
            Assert.That(field.NotSpecified, Is.True);
        }
    }
}