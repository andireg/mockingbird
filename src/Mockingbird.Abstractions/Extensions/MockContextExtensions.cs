namespace Mockingbird.Extensions
{
    public static class MockContextExtensions
    {
        public static TInstance GetInstanceOf<TInstance>(this IMockContext mockContext)
        {
            object? instance = mockContext.GetInstanceOf(typeof(TInstance));
            if (instance == null)
            {
                throw new InvalidOperationException($"Could not create instance of {typeof(TInstance).FullName}");
            }

            return (TInstance)instance;
        }
    }
}
