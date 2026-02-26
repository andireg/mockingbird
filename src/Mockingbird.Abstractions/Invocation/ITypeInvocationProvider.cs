namespace Mockingbird.Invocation
{
    public interface ITypeInvocationProvider
    {
        void AddInvocation(string functionName, object? arguments, object? result);
    }
}