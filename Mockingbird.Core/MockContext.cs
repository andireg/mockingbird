using Mockingbird.Factory;
using Mockingbird.Invocation;
using Newtonsoft.Json;

namespace Mockingbird
{
    internal sealed class MockContext<T> : IMockContext<T>
    {
        private readonly IObjectFactoryContext classFactoryContext;
        private T? instance;
        private IEnumerable<TypeInvocationInfo>? collectedInvocations;
        private readonly string snapshotFile;

        internal MockContext(IObjectFactoryContext classFactoryContext, string snapshotFile)
        {
            this.classFactoryContext = classFactoryContext;
            this.snapshotFile = snapshotFile;
        }

        public T Instance => instance ??= CreateInstance();

        public void Dispose()
        {
            IEnumerable<TypeInvocationInfo> invocations = GetTypeInvocations();
            string json = JsonConvert.SerializeObject(invocations, Formatting.Indented);
            File.WriteAllText(snapshotFile, json);
        }

        public void Verify()
        {
            IEnumerable<TypeInvocationInfo> invocations = GetTypeInvocations();
            classFactoryContext.SetupProvider.Verify(invocations);
        }

        private IEnumerable<TypeInvocationInfo> GetTypeInvocations()
        {
            if (collectedInvocations == null)
            {
                collectedInvocations = classFactoryContext.InvocationProvider.GetInvocations().OrderBy(i => i.TypeName);
            }

            return collectedInvocations;
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
