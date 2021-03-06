using Mockingbird.Tests.Services;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mockingbird.Tests
{
    public class LoggingServiceTests
    {
        [Fact]
        public async Task TestGetText()
        {
            StringBuilder stringBuilder = new();
            using IMockContext<InterfaceService> context = MockContextFactory.Start<InterfaceService>(cfg =>
            {
                cfg.LogOutput = msg => stringBuilder.AppendLine(msg);
            });
            string actual = await context.Instance.GetTextAsync("FooBar", default);
            string messages = stringBuilder.ToString();
            Assert.Equal(@"Instance of Mockingbird.Tests.Services.IInterfaceServiceArgument as Castle.Proxies.IInterfaceServiceArgumentProxy created
Instance of Mockingbird.Tests.Services.InterfaceService as Mockingbird.Tests.Services.InterfaceService created
", messages);
        }
    }
}