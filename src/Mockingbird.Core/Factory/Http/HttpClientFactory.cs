using Mockingbird.Invocation;
using Mockingbird.Output;

namespace Mockingbird.Factory.Http
{
    public class HttpClientFactory : IObjectFactory
    {
        public bool CanCreateInstance(Type type, IObjectFactoryContext context)
            => type == typeof(HttpClient);

        public object CreateInstance(Type type, IObjectFactoryContext context)
        {
            ITypeInvocationProvider typeInvocationProvider = context.InvocationProvider.ForType(type);
            context.SetupProvider.TryGetSetup(type, out TypeInvocationInfo? typeSetup);
            MockedHttpMessageHandler httpMessageHandler = new(typeSetup, typeInvocationProvider);
            return new HttpClient(httpMessageHandler);
        }
    }
}