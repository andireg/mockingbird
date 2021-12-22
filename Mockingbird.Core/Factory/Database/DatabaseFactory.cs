using Mockingbird.Invocation;
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
                return true;
            }

            instance = null;
            return false;
        }
    }
}
