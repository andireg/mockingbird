using Mockingbird.Factory;
using Mockingbird.Factory.Database;
using Mockingbird.Factory.Http;
using Mockingbird.Factory.Moq;
using Mockingbird.Setup;
using System.Runtime.CompilerServices;

namespace Mockingbird
{
    public static class MockContextFactory
    {
        public static IMockContext<T> Start<T>(
            Action<MockContextOptions>? configure = null,
            [CallerMemberName] string? methodName = null,
            [CallerFilePath] string? filePath = null)
        {
            MockContextOptions options = new();
            configure?.Invoke(options);
            string baseFilename = Path.ChangeExtension(filePath!, null);
            string setupFile = $"{baseFilename}.{methodName}.setup.json";
            string snapshotFile = $"{baseFilename}.{methodName}.snapshot.json";
            ObjectFactoryContext classFactoryContext = new(
                new ChainedFactory(
                    options.GetDefinedImplementationFactory(),
                    options.GetAddedFactories(),
                    new HttpClientFactory(), 
                    new DatabaseFactory(),
                    new MoqFactory(),
                    new ClassFactory()),
                new SetupProvider(setupFile));
            return new MockContext<T>(classFactoryContext, snapshotFile);
        }
    }
}