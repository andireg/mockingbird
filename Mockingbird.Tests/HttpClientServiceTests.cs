using Mockingbird;
using Mockingbird.Tests.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Mockingbird.Tests
{
    public class HttpClientServiceTests
    {
        [Fact]
        public async Task TestSendGetRequest()
        {
            using IMockContext<HttpClientService> context = MockContextFactory.Start<HttpClientService>();
            string actual = await context.Instance.SendGetRequestAsync("FooBar", default);
            context.Verify();
        }

        [Fact]
        public async Task TestSendPostRequest()
        {
            using IMockContext<HttpClientService> context = MockContextFactory.Start<HttpClientService>();
            string actual = await context.Instance.SendPostRequestAsync(
                new Dictionary<string, string>
                {
                        {"Key", "Value" },
                },
                default);
            context.Verify();
        }
    }
}