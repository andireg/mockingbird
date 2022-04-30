using Mockingbird.Tests.Services;
using Xunit;
using System.Linq;

namespace Mockingbird.Tests
{
    public class RefAndOutTests
    {
        [Fact]
        public void TestRefMethod()
        {
            Moq.Mock<IRefService> refService = new();
            Moq.Mock<IOutService> outService = new();

            string text3 = "TEXT";
            refService.Setup(s => s.RefMethod(Moq.It.Is<decimal>(x => x == 100), ref text3)).Returns(true);
            RefAndOutService refAndOutService = new(refService.Object, outService.Object);

            string text2 = "TEXT";
            bool actual2 = refAndOutService.RefMethod(100, ref text2);



            Assert.True(actual2);


            using IMockContext<RefAndOutService> context = MockContextFactory.Start<RefAndOutService>();
            string text = "TEXT";
            bool actual = context.Instance.RefMethod(100, ref text);
            Assert.True(actual);
            context.Verify();
        }

        [Fact]
        public void TestOutMethod()
        {
            using IMockContext<RefAndOutService> context = MockContextFactory.Start<RefAndOutService>();
            string text = "TEXT";
            bool actual = context.Instance.OutMethod(100, out text);
            Assert.True(actual);
            context.Verify();
        }
    }
}
