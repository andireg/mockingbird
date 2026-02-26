namespace Mockingbird.Output
{
    public static class LogOutputExtensions
    {
        public static void InstanceCreated(this Action<string> logOutput, Type instanceType, string factoryName)
        {
            logOutput.Invoke($"Instance {instanceType.FullName} created by {factoryName}");
        }
    }
}