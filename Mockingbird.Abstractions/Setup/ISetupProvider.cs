using Mockingbird.Invocation;

namespace Mockingbird.Setup
{
    public interface ISetupProvider
    {
        bool TryGetSetup(Type type, out TypeInvocationInfo? typeSetup);

        void Verify(IEnumerable<TypeInvocationInfo> invocations);
    }
}
