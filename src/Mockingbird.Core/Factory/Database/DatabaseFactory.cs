using Mockingbird.Invocation;
using System.Data;

namespace Mockingbird.Factory.Database
{
    public class DatabaseFactory : IObjectFactory
    {
        public bool CanCreateInstance(Type type, IObjectFactoryContext context)
            => type == typeof(IDbConnection);

        public object CreateInstance(Type type, IObjectFactoryContext context)
        {
            ITypeInvocationProvider typeInvocationProvider = context.InvocationProvider.ForType(type);
            context.SetupProvider.TryGetSetup(type, out TypeInvocationInfo? typeSetup);
            return new MockedDbConnection(typeSetup, typeInvocationProvider);
        }
    }
}