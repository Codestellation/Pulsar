﻿using System;
using System.Linq;
using Codestellation.Pulsar.Cron;
using NUnit.Framework;

namespace Codestellation.Pulsar.Tests.Cron
{
    [TestFixture]
    public class CronDayScheduleTests
    {
        private SimpleCronField _second;
        private SimpleCronField _minute;
        private SimpleCronField _hour;
        private CronDaySchedule _schedule;

        [SetUp]
        public void SetUp()
        {
            _second = SimpleCronField.ParseSeconds("0/10");
            _minute = SimpleCronField.ParseMinutes("0,1");
            _hour = SimpleCronField.ParseHours("10");
            _schedule = new CronDaySchedule(_second, _minute, _hour);
        }

        [Test]
        public void Should_enumerate_intraday_points()
        {
            var actual = _schedule.Values.ToArray();

            var initial = new TimeSpan(10, 0, 0);
            var expected = new[]
            {
                initial.Add(TimeSpan.FromSeconds(00)),
                initial.Add(TimeSpan.FromSeconds(10)),
                initial.Add(TimeSpan.FromSeconds(20)),
                initial.Add(TimeSpan.FromSeconds(30)),
                initial.Add(TimeSpan.FromSeconds(40)),
                initial.Add(TimeSpan.FromSeconds(50)),
                initial.Add(TimeSpan.FromSeconds(60)),
                initial.Add(TimeSpan.FromSeconds(70)),
                initial.Add(TimeSpan.FromSeconds(80)),
                initial.Add(TimeSpan.FromSeconds(90)),
                initial.Add(TimeSpan.FromSeconds(100)),
                initial.Add(TimeSpan.FromSeconds(110)),
            };
            //note: order of values does matter!
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Should_find_closest_time_if_now_is_before_occurence()
        {
            var now = new TimeSpan(9, 0, 0);

            TimeSpan result;
            var found = _schedule.TryGetTimeAfter(now, out result);

            Assert.That(found, Is.True);
            var expectedTime = new TimeSpan(10, 0, 0);
            Assert.That(result, Is.EqualTo(expectedTime));
        }

        [Test]
        public void Should_find_closest_time_if_now_is_equals_to_a_point()
        {
            var now = new TimeSpan(10, 0, 0);

            TimeSpan result;
            var found = _schedule.TryGetTimeAfter(now, out result);

            Assert.That(found, Is.True);
            var expectedTime = new TimeSpan(10, 0, 0);
            Assert.That(result, Is.EqualTo(expectedTime));
        }

        [Test]
        public void Should_find_closest_time_if_now_is_between_points()
        {
            var now = new TimeSpan(10, 0, 1);

            TimeSpan result;
            var found = _schedule.TryGetTimeAfter(now, out result);

            Console.WriteLine(result);
            Assert.That(found, Is.True);
            var expectedTime = new TimeSpan(10, 0, 10);
            Assert.That(result, Is.EqualTo(expectedTime));
        }

        [Test]
        public void Should_not_find_closest_time_if_now_is_after_last_point()
        {
            var now = new TimeSpan(10, 0, 51);

            TimeSpan result;
            var found = _schedule.TryGetTimeAfter(now, out result);

            Console.WriteLine(result);
            var expectedTime = new TimeSpan(10, 1, 0);
            Assert.That(found, Is.True);
            Assert.That(result, Is.EqualTo(expectedTime));
        }
    }
}