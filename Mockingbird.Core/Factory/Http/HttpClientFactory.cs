﻿using Mockingbird.Invocation;

namespace Mockingbird.Factory.Http
{
    public class HttpClientFactory : IObjectFactory
    {
        public bool TryCreateInstance(Type type, IObjectFactoryContext context, out object? instance)
        {
            if (type == typeof(HttpClient))
            {
                ITypeInvocationProvider typeInvocationProvider = context.InvocationProvider.ForType(type);
                context.SetupProvider.TryGetSetup(type, out TypeInvocationInfo? typeSetup);
                MockedHttpMessageHandler httpMessageHandler = new(typeSetup, typeInvocationProvider);
                instance = new HttpClient(httpMessageHandler);
                return true;
            }

            instance = null;
            return false;
        }
    }
}
