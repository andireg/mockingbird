namespace Mockingbird.Invocation
{
    public record TypeInvocationInfo(string TypeName)
    {
        public IList<InvocationInfo> Invocations { get; } = new List<InvocationInfo>();
    }
}