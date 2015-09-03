using System;
using System.Diagnostics;
using Codestellation.Pulsar.Cron;
using NUnit.Framework;

namespace Codestellation.Pulsar.Tests.Cron
{
    [TestFixture]
    public class CronExpressionTests
    {
        [TestCase("0 0 10 * * ?")]
        [TestCase("1-10 30 * 1,11,21 3,4-8 L 2000-2100")]
        public void Should_correctly_parse_expressions(string expression)
        {
            Assert.DoesNotThrow(() => new CronExpression(expression));
        }

        [TestCase(10, 15)] //before 12am - this day
        [TestCase(15, 16)] //after 12am - next day
        public void Should_correctly_show_nearest_dayly_occurence(int hour, int expecatedDay)
        {
            var datetime = new DateTime(2012, 10, 15).AddHours(hour);

            var expression = new CronExpression("0 0 10 * * ?");

            var nearest = expression.NearestAfter(datetime);

            var expected = new DateTime(2012, 10, expecatedDay, 10, 0, 0);

            Assert.That(nearest.Value, Is.EqualTo(expected));
        }

        [TestCase(1, 2012)]
        [TestCase(12, 2013)]
        public void Should_correctly_show_nearest_occurence2(int currentMonth, int expectedYear)
        {
            var datetime = new DateTime(2012, currentMonth, 15);

            var expression = new CronExpression("0 0 12 L 3,6 ?");

            var nearest = expression.NearestAfter(datetime);

            var expected = new DateTime(expectedYear, 3, 31, 12, 0, 0);

            Assert.That(nearest.Value, Is.EqualTo(expected));
        }

        [Test]
        public void Should_correctly_handle_datetime_with_microseconds()
        {
            var datetime = new DateTime(2012, 10, 15, 10, 0, 0).AddMilliseconds(100);

            var expression = new CronExpression("* 0 10 * * ?");

            var nearest = expression.NearestAfter(datetime);

            var expected = new DateTime(2012, 10, 15, 10, 0, 1);

            Assert.That(nearest.Value, Is.EqualTo(expected));
        }

        [TestCase(DateTimeKind.Utc)]
        [TestCase(DateTimeKind.Local)]
        [TestCase(DateTimeKind.Unspecified)]
        public void Should_return_same_date_time_kind_as_was_supplied(DateTimeKind expected)
        {
            var datetime = new DateTime(2012, 10, 15, 10, 0, 0, expected).AddMilliseconds(100);
            var expression = new CronExpression("* 0 10 * * ?");
            var nearest = expression.NearestAfter(datetime);
            var actual = nearest.Value;
            Assert.That(actual.Kind, Is.EqualTo(expected));
        }

        [Test]
        public void Perfomance_test_worst_case()
        {
            var expression = new CronExpression("* * * * * ?");
            var datetime = new DateTime(2012, 12, 31, 23, 59, 59);

            var nearestAfter = expression.NearestAfter(datetime);
            Console.WriteLine(nearestAfter);
            Assert.That(nearestAfter, Is.Not.Null);

            var watch = Stopwatch.StartNew();

            const int i1 = 1000 * 1000;
            for (int i = 0; i < i1; i++)
            {
                expression.NearestAfter(datetime);
            }

            watch.Stop();

            var opsec = i1 * 1.0 / (watch.ElapsedMilliseconds / 1000.0);
            var mscPerOp = 1 / opsec * 1000 * 1000;
            Console.WriteLine("Elapsted {0}; {1:N2} op/sec; {2} mcs", watch.Elapsed, opsec, mscPerOp);
        }

        [Test]
        public void Perfomance_test_best_case()
        {
            var expression = new CronExpression("0 0 0 1 * ?");
            var datetime = new DateTime(2012, 1, 1, 0, 0, 0);

            var nearestAfter = expression.NearestAfter(datetime);
            Console.WriteLine(nearestAfter);
            Assert.That(nearestAfter, Is.Not.Null);

            var watch = Stopwatch.StartNew();

            const int i1 = 1000 * 1000;
            for (int i = 0; i < i1; i++)
            {
                expression.NearestAfter(datetime);
            }

            watch.Stop();

            var opsec = i1 * 1.0 / (watch.ElapsedMilliseconds / 1000.0);
            var mscPerOp = 1 / opsec * 1000 * 1000;
            Console.WriteLine("Elapsted {0}; {1:N2} op/sec; {2} mcs", watch.Elapsed, opsec, mscPerOp);
        }
    }
}