namespace Mockingbird.Factory
{
    internal class ChainedFactory : IObjectFactory
    {
        private readonly IEnumerable<IObjectFactory> classFactories;

        public ChainedFactory(params IObjectFactory[] classFactories)
        {
            this.classFactories = classFactories;
        }

        public virtual bool CanCreateInstance(Type type, IObjectFactoryContext context)
            => classFactories.Any(factory => factory.CanCreateInstance(type, context));

        public virtual object CreateInstance(Type type, IObjectFactoryContext context)
        {
            foreach (IObjectFactory classFactory in classFactories)
            {
                if (classFactory.CanCreateInstance(type, context))
                {
                    return classFactory.CreateInstance(type, context);
                }
            }

            return new NotSupportedException($"Could not create instance of {type.FullName}");
        }
    }
}