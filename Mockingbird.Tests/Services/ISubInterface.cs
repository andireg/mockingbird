namespace Mockingbird.Tests.Services
{
    public interface ISubInterface : IMainInterface
    {

        decimal Number { get; set; }

        decimal GetNumber();
    }
}
