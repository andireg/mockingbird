using Mockingbird.Factory;
using Mockingbird.Invocation;
using Newtonsoft.Json;

namespace Mockingbird
{
    internal sealed class MockContext<T> : IMockContext<T>
    {
        private readonly IObjectFactoryContext classFactoryContext;
        private T? instance;
        private readonly string snapshotFile;

        internal MockContext(IObjectFactoryContext classFactoryContext, string snapshotFile)
        {
            this.classFactoryContext = classFactoryContext;
            this.snapshotFile = snapshotFile;
        }

        public T Instance => instance ??= CreateInstance();

        public void Dispose()
        {
            IEnumerable<TypeInvocationInfo> invocations = classFactoryContext.InvocationProvider.GetInvocations().OrderBy(i => i.TypeName);
            string json = JsonConvert.SerializeObject(invocations, Formatting.Indented);
            File.WriteAllText(snapshotFile, json);
        }

        public void Verify()
        {
            IEnumerable<TypeInvocationInfo> invocations = classFactoryContext.InvocationProvider.GetInvocations().OrderBy(i => i.TypeName);
            classFactoryContext.SetupProvider.Verify(invocations);
        }

        private T CreateInstance()
        {
            if (classFactoryContext.RootFactory.TryCreateInstance(typeof(T), classFactoryContext, out object? inst))
            {
                return (T)inst!;
            }

            throw new NotImplementedException($"Could not create instance for type {typeof(T).FullName}");
        }
    }
}
