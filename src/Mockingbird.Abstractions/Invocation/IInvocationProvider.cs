namespace Mockingbird.Invocation
{
    public interface IInvocationProvider
    {
        ITypeInvocationProvider ForType(Type type);

        IEnumerable<TypeInvocationInfo> GetInvocations();

        void BeforeCollectInvocations(Action callback);
    }
}