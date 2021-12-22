namespace Mockingbird.Factory
{
    public interface IObjectFactory
    {
        bool TryCreateInstance(Type type, IObjectFactoryContext context, out object? instance);
    }
}