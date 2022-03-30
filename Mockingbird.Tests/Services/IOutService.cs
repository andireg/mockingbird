namespace Mockingbird.Tests.Services
{
    public interface IOutService
    {
        bool OutMethod(decimal number, out string text);
    }
}
