using Newtonsoft.Json;

namespace Mockingbird.Invocation
{
    internal static class InvocationExtensions
    {
        public static InvocationInfo? GetInvocation(this TypeInvocationInfo? typeInvocationInfo, string invocationName, object arguments) 
            => typeInvocationInfo?.Invocations?.FirstOrDefault(invocation =>
                invocation.InvocationName == invocationName &&
                JsonConvert.SerializeObject(invocation.Arguments) == JsonConvert.SerializeObject(arguments));
    }
}
