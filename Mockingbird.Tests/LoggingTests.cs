using Mockingbird.Factory;
using Mockingbird.Tests.Services;
using Moq;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

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
            Assert.Equal(@"Instance Mockingbird.Tests.Services.IInterfaceServiceArgument created by MoqFactory
Instance Mockingbird.Tests.Services.InterfaceService created by ClassFactory
", messages);
        }
    }
}
