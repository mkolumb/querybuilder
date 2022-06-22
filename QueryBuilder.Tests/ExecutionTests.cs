using System;
using SqlKata.Net6.Execution;
using Xunit;

namespace SqlKata.Net6.Tests
{
    public class ExecutionTests
    {
        [Fact]
        public void ShouldThrowException()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                new Query("Books").Get();
            });
        }

        [Fact]
        public void TimeoutShouldBeCarriedToNewCreatedFactory()
        {
            var db = new QueryFactory();
            db.QueryTimeout = 4000;
            var newFactory = QueryExtensions.CreateQueryFactory(db.Query());
            Assert.Equal(db.QueryTimeout, newFactory.QueryTimeout);
        }
    }
}
