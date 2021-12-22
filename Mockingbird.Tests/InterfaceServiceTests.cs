using Mockingbird;
using Mockingbird.Tests.Services;
using System.Threading.Tasks;
using Xunit;

namespace Mockingbird.Tests
{
    public class InterfaceServiceTests
    {
        [Fact]
        public async Task TestSendGetRequest()
        {
            using IMockContext<InterfaceService> context = MockContextFactory.Start<InterfaceService>();
            string actual = await context.Instance.GetTextAsync("FooBar", default);
            Assert.Equal("FOOBAR", actual);
            context.Verify();
        }
    }
}
