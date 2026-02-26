namespace Mockingbird.Tests.Services
{
    public interface IRefService
    {
        bool RefMethod(decimal number, ref string text);
    }
}
