namespace Mockingbird
{
    public interface IMockContext<T> : IDisposable
    {
        T Instance { get; }

        void Verify();
    }
}
