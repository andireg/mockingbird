using Mockingbird.Output;

namespace Mockingbird.Factory
{
    internal class DefinedImplementationFactory : IObjectFactory
    {
        private readonly Dictionary<Type, Func<IObjectFactoryContext, object>> implementationFactories;

        public DefinedImplementationFactory(Dictionary<Type, Func<IObjectFactoryContext, object>> implementationFactories)
        {
            this.implementationFactories = implementationFactories;
        }

        public bool CanCreateInstance(Type type, IObjectFactoryContext context)
            => implementationFactories.ContainsKey(type);

        public object CreateInstance(Type type, IObjectFactoryContext context)
        {
            Func<IObjectFactoryContext, object> factory = implementationFactories[type];
            return factory.Invoke(context);
        }
    }
}