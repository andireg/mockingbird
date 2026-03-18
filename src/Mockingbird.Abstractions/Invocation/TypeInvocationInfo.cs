namespace Mockingbird.Invocation;

public record TypeInvocationInfo(string TypeName)
{
    public List<InvocationInfo> Invocations { get; set; } = new();
}