namespace Mockingbird.Factory
{
    public interface IObjectFactory
    {
        object CreateInstance(Type type, IObjectFactoryContext context);

        bool CanCreateInstance(Type type, IObjectFactoryContext context);
    }
}