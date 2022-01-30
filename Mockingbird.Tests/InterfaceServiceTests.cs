using Mockingbird.Factory;
using Mockingbird.Tests.Services;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Mockingbird.Tests
{
    public class InterfaceServiceTests
    {
        [Fact]
        public async Task TestGetText()
        {
            using IMockContext<InterfaceService> context = MockContextFactory.Start<InterfaceService>();
            string actual = await context.Instance.GetTextAsync("FooBar", default);
            Assert.Equal("FOOBAR", actual);
            context.Verify();
        }

        [Fact]
        public async Task TestGetText_UseImplementation()
        {
            Mock<IInterfaceServiceArgument> mockedInterfaceServiceArgument = new();
            mockedInterfaceServiceArgument.Setup(mock => mock.GetTextAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync("HELLO-WORLD");
            using IMockContext<InterfaceService> context = MockContextFactory.Start<InterfaceService>(options =>
            {
                options.AddImplementation(typeof(IInterfaceServiceArgument), mockedInterfaceServiceArgument.Object);
            });

            string actual = await context.Instance.GetTextAsync("FooBar", default);
            Assert.Equal("HELLO-WORLD", actual);
            context.Verify();
        }

        [Fact]
        public async Task TestGetText_UseFactory()
        {
            Mock<IInterfaceServiceArgument> mockedInterfaceServiceArgument = new();
            mockedInterfaceServiceArgument.Setup(mock => mock.GetTextAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync("HELLO-WORLD");
            object? instance = mockedInterfaceServiceArgument.Object;
            Mock<IObjectFactory> objectFactoryMock = new();
            objectFactoryMock
                .Setup(f => f.CanCreateInstance(
                    It.Is<Type>(m => m == typeof(IInterfaceServiceArgument)),
                    It.IsAny<IObjectFactoryContext>()))
                .Returns(true);
            objectFactoryMock
                .Setup(f => f.CreateInstance(
                    It.Is<Type>(m => m == typeof(IInterfaceServiceArgument)),
                    It.IsAny<IObjectFactoryContext>()))
                .Returns(instance);

            using IMockContext<InterfaceService> context = MockContextFactory.Start<InterfaceService>(options =>
            {
                options.AddFactory(objectFactoryMock.Object);
            });

            string actual = await context.Instance.GetTextAsync("FooBar", default);
            Assert.Equal("HELLO-WORLD", actual);
            context.Verify();
        }
    }
}