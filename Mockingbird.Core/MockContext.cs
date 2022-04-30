using Mockingbird.Factory;
using Mockingbird.Invocation;
using Mockingbird.Utils;
using Newtonsoft.Json;

namespace Mockingbird
{
    internal sealed class MockContext<T> : IMockContext<T>
    {
        private readonly IObjectFactoryContext classFactoryContext;
        private T? instance;
        private IEnumerable<TypeInvocationInfo>? collectedInvocations;
        private readonly string snapshotFile;
        private readonly string setupFile;

        internal MockContext(IObjectFactoryContext classFactoryContext, string snapshotFile, string setupFile)
        {
            this.classFactoryContext = classFactoryContext;
            this.snapshotFile = snapshotFile;
            this.setupFile = setupFile;
        }

        public T Instance => instance ??= CreateInstance();

        public void Dispose()
        {
            IEnumerable<TypeInvocationInfo> invocations = GetTypeInvocations();
            string json = JsonUtils.SerializeObject(invocations, Formatting.Indented);
            File.WriteAllText(snapshotFile, json);

            if (!File.Exists(setupFile))
            {
                File.WriteAllText(setupFile, json);
            }
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
            if (classFactoryContext.RootFactory.CanCreateInstance(typeof(T), classFactoryContext))
            {
                return (T)classFactoryContext.RootFactory.CreateInstance(typeof(T), classFactoryContext);
            }


            throw new NotImplementedException($"Could not create instance for type {typeof(T).FullName}");
        }

        public object? GetInstanceOf(Type type)
        {
            if (classFactoryContext.RootFactory.CanCreateInstance(type, classFactoryContext))
            {
                return classFactoryContext.RootFactory.CreateInstance(type, classFactoryContext);
            }

            return null;
        }
    }
}