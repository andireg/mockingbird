using Mockingbird.Tests.Services;
using Xunit;

namespace Mockingbird.Tests
{
    public class RefAndOutTests
    {
        [Fact]
        public void TestRefMethod()
        {
            using IMockContext<RefAndOutService> context = MockContextFactory.Start<RefAndOutService>();
            string text = "TEXT";
            bool actual = context.Instance.RefMethod(100, ref text);
            context.Verify();
        }

        [Fact]
        public void TestOutMethod()
        {
            using IMockContext<RefAndOutService> context = MockContextFactory.Start<RefAndOutService>();
            string text = "TEXT";
            bool actual = context.Instance.OutMethod(100, out text);
            context.Verify();
        }
    }
}
