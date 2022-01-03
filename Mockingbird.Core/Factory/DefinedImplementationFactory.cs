namespace Mockingbird.Factory
{
    internal class DefinedImplementationFactory : IObjectFactory
    {
        private Dictionary<Type, Func<IObjectFactoryContext, object>> implementationFactories;

        public DefinedImplementationFactory(Dictionary<Type, Func<IObjectFactoryContext, object>> implementationFactories)
        {
            this.implementationFactories = implementationFactories;
        }

        public bool TryCreateInstance(Type type, IObjectFactoryContext context, out object? instance)
        {
            if (implementationFactories.TryGetValue(type, out Func<IObjectFactoryContext, object>? factory))
            {
                instance = factory!.Invoke(context);
                return true;
            }

            instance = null;
            return false;
        }
    }
}
