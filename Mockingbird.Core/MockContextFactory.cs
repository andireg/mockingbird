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
            [CallerMemberName] string? methodName = null,
            [CallerFilePath] string? filePath = null)
        {
            string baseFilename = Path.ChangeExtension(filePath!, null);
            string setupFile = $"{baseFilename}.{methodName}.setup.json";
            string snapshotFile = $"{baseFilename}.{methodName}.snapshot.json";
            ObjectFactoryContext classFactoryContext = new(
                new ChainedFactory(
                    new HttpClientFactory(), 
                    new DatabaseFactory(),
                    new MoqFactory(),
                    new ClassFactory()),
                new SetupProvider(setupFile));
            return new MockContext<T>(classFactoryContext, snapshotFile);
        }
    }
}