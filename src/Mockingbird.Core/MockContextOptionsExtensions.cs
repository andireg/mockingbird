namespace Mockingbird
{
    public static class MockContextOptionsExtensions
    {
        public static MockContextOptions AddImplementation(this MockContextOptions options, Type type, object instance)
        {
            options.AddImplementation(type, _ => instance);
            return options;
        }
    }
}