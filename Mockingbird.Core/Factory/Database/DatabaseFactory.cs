using Mockingbird.Invocation;
using Mockingbird.Output;
using System.Data;

namespace Mockingbird.Factory.Database
{
    public class DatabaseFactory : IObjectFactory
    {
        public bool TryCreateInstance(Type type, IObjectFactoryContext context, out object? instance)
        {
            if (type == typeof(IDbConnection))
            {
                ITypeInvocationProvider typeInvocationProvider = context.InvocationProvider.ForType(type);
                context.SetupProvider.TryGetSetup(type, out TypeInvocationInfo? typeSetup);
                instance = new MockedDbConnection(typeSetup, typeInvocationProvider);
                context.LogOutput.InstanceCreated(type, nameof(DatabaseFactory));
                return true;
            }

            instance = null;
            return false;
        }
    }
}
