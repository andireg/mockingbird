using Mockingbird.Invocation;
using Mockingbird.Setup;

namespace Mockingbird.Factory
{
    internal class ObjectFactoryContext : IObjectFactoryContext
    {
        public ObjectFactoryContext(IObjectFactory rootFactory, ISetupProvider setupProvider)
        {
            RootFactory = rootFactory;
            SetupProvider = setupProvider;
        }

        public IObjectFactory RootFactory { get; }

        public ISetupProvider SetupProvider { get; }

        public IInvocationProvider InvocationProvider { get; } = new InvocationProvider();
    }
}