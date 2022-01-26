namespace Mockingbird.Factory
{
    internal class ChainedFactory : IObjectFactory
    {
        private readonly IEnumerable<IObjectFactory> classFactories;

        public ChainedFactory(params IObjectFactory[] classFactories)
        {
            this.classFactories = classFactories;
        }

        public bool TryCreateInstance(Type type, IObjectFactoryContext context, out object? instance)
        {
            foreach (IObjectFactory classFactory in classFactories)
            {
                if (classFactory.TryCreateInstance(type, context, out instance))
                {
                    return true;
                }
            }

            instance = null;
            return false;
        }
    }
}