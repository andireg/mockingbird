using Mockingbird.Factory;

namespace Mockingbird
{
    public class MockContextOptions
    {
        private readonly Dictionary<Type, Func<IObjectFactoryContext, object>> implementationFactories = new();
        private readonly List<IObjectFactory> objectFactories = new();

        public Action<string>? LogOutput { get; set; }

        public void AddImplementation(Type type, Func<IObjectFactoryContext, object> factory)
            => implementationFactories.Add(type, factory);

        public void AddFactory(IObjectFactory factory)
            => objectFactories.Add(factory);

        internal DefinedImplementationFactory GetDefinedImplementationFactory()
            => new DefinedImplementationFactory(implementationFactories);

        internal IObjectFactory GetAddedFactories() 
            => new ChainedFactory(objectFactories.ToArray());
    }
}