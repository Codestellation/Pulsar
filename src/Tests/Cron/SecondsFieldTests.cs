﻿using System.Linq;
using Codestellation.Pulsar.Cron;
using NUnit.Framework;

namespace Codestellation.Pulsar.Tests.Cron
{
    [TestFixture]
    public class SecondsFieldTests
    {
        ///
        [Test]
        public void Can_parse_simple_seconds()
        {
            var field = SecondsField.Parse("42");
            CollectionAssert.AreEqual(new[]{42}, field.Values);
        }
        
        [Test]
        public void Can_parse_comma_separated_values()
        {
            var field = SecondsField.Parse("5,42,12");
            CollectionAssert.AreEqual(new[]{5,12, 42}, field.Values);
        }
        
        [Test]
        public void Can_parse_all_value()
        {
            var field = SecondsField.Parse("*");
            var expected = Enumerable.Range(0, 59);
            CollectionAssert.AreEqual(expected, field.Values);
        }
        
        
        [Test]
        public void Can_parse_ranges_value()
        {
            var field = SecondsField.Parse("1-3,20-22,42");
            
            CollectionAssert.AreEqual(new[]{1,2,3,20,21,22,42} , field.Values);
        }
        
        [Test]
        public void Can_parse_increment_values()
        {
            var field = SecondsField.Parse("0-3,20/5");
            
            CollectionAssert.AreEqual(new[]{0,1,2,3,20,25,30,35,40,45,50,55} , field.Values);
        }
    }
}