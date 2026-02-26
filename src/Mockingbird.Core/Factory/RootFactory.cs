namespace Mockingbird.Factory
{
    internal class RootFactory : ChainedFactory
    {
        public RootFactory(params IObjectFactory[] classFactories) 
            : base(classFactories)
        {

        }

        public override object CreateInstance(Type type, IObjectFactoryContext context)
        {
            try
            {
                object instance = base.CreateInstance(type, context);
                context.LogOutput($"Instance of {type.FullName} as {instance.GetType().FullName} created");
                return instance;
            }
            catch (Exception ex)
            {
                context.LogOutput($"Could not create instance of {type.FullName} because {ex.Message}");
                throw;
            }
        }
    }
}