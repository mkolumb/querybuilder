using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using SqlKata.Net6.Compilers;
using SqlKata.Net6.Tests.Infrastructure;
using Xunit;

namespace SqlKata.Net6.Tests
{
    public class ParameterTypeTests : TestSupport
    {
        public enum EnumExample
        {
            First,
            Second,
            Third,
        }

        public class ParameterTypeGenerator : IEnumerable<object[]>
        {
            private readonly List<object[]> _data = new List<object[]>
            {
                new object[] {"1", 1},
                new object[] {Convert.ToSingle("10.5", CultureInfo.InvariantCulture).ToString(), 10.5},
                new object[] {"-2", -2},
                new object[] {Convert.ToSingle("-2.8", CultureInfo.InvariantCulture).ToString(), -2.8},
                new object[] {"cast(1 as bit)", true},
                new object[] {"cast(0 as bit)", false},
                new object[] {"'2018-10-28 19:22:00'", new DateTime(2018, 10, 28, 19, 22, 0)},
                new object[] {"0 /* First */", EnumExample.First},
                new object[] {"1 /* Second */", EnumExample.Second},
                new object[] {"'a string'", "a string"},
            };

            public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(ParameterTypeGenerator))]
        public void CorrectParameterTypeOutput(string rendered, object input)
        {
            var query = new Query("Table").Where("Col", input);

            var c = Compile(query);

            Assert.Equal($"SELECT * FROM [Table] WHERE [Col] = {rendered}", c[EngineCodes.SqlServer]);
        }
    }
}
