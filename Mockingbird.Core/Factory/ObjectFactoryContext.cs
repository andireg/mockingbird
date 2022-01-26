using Mockingbird.Invocation;
using Mockingbird.Setup;

namespace Mockingbird.Factory
{
    internal class ObjectFactoryContext : IObjectFactoryContext
    {
        public ObjectFactoryContext(IObjectFactory rootFactory, ISetupProvider setupProvider, Action<string>? logOutput)
        {
            RootFactory = rootFactory;
            SetupProvider = setupProvider;
            LogOutput = logOutput == null ? _ => { } : logOutput;
        }

        public IObjectFactory RootFactory { get; }

        public ISetupProvider SetupProvider { get; }

        public IInvocationProvider InvocationProvider { get; } = new InvocationProvider();

        public Action<string> LogOutput { get; }
    }
}