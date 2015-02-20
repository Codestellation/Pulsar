using System;
using System.Linq;
using Codestellation.Pulsar.Cron;
using NUnit.Framework;

namespace Codestellation.Pulsar.Tests.Cron
{
    [TestFixture]
    public class DayOfMonthFieldTests
    {
        [TestCase(2011, 1, 31)]
        [TestCase(2012, 2, 29)]
        [TestCase(2011, 2, 28)]
        [TestCase(2011, 3, 31)]
        [TestCase(2011, 4, 30)]
        [TestCase(2011, 5, 31)]
        [TestCase(2011, 6, 30)]
        [TestCase(2011, 7, 31)]
        [TestCase(2011, 8, 31)]
        [TestCase(2011, 9, 30)]
        [TestCase(2011, 10, 31)]
        [TestCase(2011, 11, 30)]
        [TestCase(2011, 12, 31)]
        public void Can_parse_single_all_values(int year, int month, int maxValue)
        {
            var field = DayOfMonthField.Parse("*");

            var values = field.GetValues(year, month);

            var expected = Enumerable.Range(1, maxValue).ToArray();
            CollectionAssert.AreEqual(expected, values);
        }

        //, - * ? / L W
        [Test]
        public void Can_parse_list_of_value()
        {
            var field = DayOfMonthField.Parse("10,20");

            var values = field.GetValues(2012, 2);

            var expected = new[] { 10, 20 };

            CollectionAssert.AreEqual(expected, values);
        }
        
        [Test]
        public void Can_parse_ranges()
        {
            var field = DayOfMonthField.Parse("10,20-22");

            var values = field.GetValues(2012, 2);

            var expected = new[] { 10, 20, 21, 22 };

            CollectionAssert.AreEqual(expected, values);
        }
        
        [Test]
        public void Can_parse_ranges_increment()
        {
            var field = DayOfMonthField.Parse("10,20-22,20/5");

            var values = field.GetValues(2012, 2);

            var expected = new[] { 10, 20, 21, 22, 25 };

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
            var date = new DateTime(2012, month, 1);

            var field = DayOfMonthField.Parse("L");

            var closestDay = field.GetClosestTo(date);

            Assert.That(closestDay, Is.EqualTo(lastDay));
        }       
        
        [TestCase(1, 2)]
        [TestCase(7, 9)]
        [TestCase(10, 10)]
        [TestCase(28, -1)]
        public void Can_parse_week_day(int day, int lastDay)
        {
            var date = new DateTime(2015, 2, day);

            var field = DayOfMonthField.Parse("W");

            var closestDay = field.GetClosestTo(date);

            Assert.That(closestDay, Is.EqualTo(lastDay));
        }
        
        [TestCase(1, 27)]
        [TestCase(28, -1)]
        public void Can_parse_last_week_day(int day, int lastDay)
        {
            var date = new DateTime(2015, 2, day);

            var field = DayOfMonthField.Parse("LW");

            var closestDay = field.GetClosestTo(date);

            Assert.That(closestDay, Is.EqualTo(lastDay));
        }
    }
}