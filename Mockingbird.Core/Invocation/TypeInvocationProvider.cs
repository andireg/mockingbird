using Newtonsoft.Json;

namespace Mockingbird.Invocation
{
    internal class TypeInvocationProvider : ITypeInvocationProvider
    {
        private readonly TypeInvocationInfo typeInvocationInfo;

        public TypeInvocationProvider(Type type)
        {
            typeInvocationInfo = new TypeInvocationInfo(type.FullName!);
        }

        public void AddInvocation(string invocationName, object? arguments, object? result)
        {
            string key = GetKey(invocationName, arguments);
            InvocationInfo? invocationInfo = typeInvocationInfo.Invocations.FirstOrDefault(i => GetKey(i.InvocationName, i.Arguments) == key);
            if (invocationInfo != null)
            {
                invocationInfo.Number++;
            }
            else
            {
                typeInvocationInfo.Invocations.Add(new InvocationInfo(invocationName, arguments, result));
            }
        }

        internal TypeInvocationInfo GetInvocations() => typeInvocationInfo;

        private static string GetKey(string invocationName, object? arguments)
            => $"{invocationName}:{(arguments == null ? string.Empty : JsonConvert.SerializeObject(arguments))}";
    }
}