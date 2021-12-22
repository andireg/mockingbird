namespace Mockingbird.Invocation
{
    public record InvocationInfo(string InvocationName, object? Arguments, object? Result)
    {
        public int Number { get; set; } = 1;
    }
}
