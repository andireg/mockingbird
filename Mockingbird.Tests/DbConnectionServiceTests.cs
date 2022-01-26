using Mockingbird.Tests.Services;
using Snapshooter.Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Mockingbird.Tests
{
    public class DbConnectionServiceTests
    {
        [Fact]
        public async Task TestQuery()
        {
            using IMockContext<DbConnectionService> context = MockContextFactory.Start<DbConnectionService>();
            IEnumerable<DbConnectionItem> actual = await context.Instance.QueryAsync("KEY", default);
            Snapshot.Match(actual);
            context.Verify();
        }

        [Fact]
        public async Task TestExecuteScalar()
        {
            using IMockContext<DbConnectionService> context = MockContextFactory.Start<DbConnectionService>();
            long actual = await context.Instance.ExecuteScalarAsync("KEY", default);
            Assert.Equal(42, actual);
            context.Verify();
        }
    }
}