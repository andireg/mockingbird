namespace Mockingbird
{
    public interface IMockContext : IDisposable
    {
        object? GetInstanceOf(Type type);

        void Verify();
    }

    public interface IMockContext<T> : IMockContext
    {
        T Instance { get; }
    }
}