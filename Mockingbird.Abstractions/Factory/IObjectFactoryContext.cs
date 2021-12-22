using Mockingbird.Invocation;
using Mockingbird.Setup;

namespace Mockingbird.Factory
{
    public interface IObjectFactoryContext
    {
        IObjectFactory RootFactory { get; }

        ISetupProvider SetupProvider { get; }
        
        IInvocationProvider InvocationProvider { get; }
    }
}