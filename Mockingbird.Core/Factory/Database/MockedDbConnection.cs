using Mockingbird.Invocation;
using System.Data;

namespace Mockingbird.Factory.Database
{
    internal class MockedDbConnection : IDbConnection
    {
        private readonly TypeInvocationInfo? typeSetup;
        private readonly ITypeInvocationProvider typeInvocationProvider;

        public MockedDbConnection(TypeInvocationInfo? typeSetup, ITypeInvocationProvider typeInvocationProvider)
        {
            this.typeSetup = typeSetup;
            this.typeInvocationProvider = typeInvocationProvider;
        }

        public string ConnectionString { get; set; }

        public int ConnectionTimeout { get; set; }

        public string Database { get; set; }

        public ConnectionState State { get; set; } = ConnectionState.Open;

        public IDbTransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            throw new NotImplementedException();
        }

        public void ChangeDatabase(string databaseName)
        {
            // nothing to do
        }

        public void Close()
        {
            // nothing to do
        }

        public IDbCommand CreateCommand()
            => new MockedDbCommand(typeSetup, typeInvocationProvider);

        public void Dispose()
        {
            // nothing to do
        }

        public void Open()
        {
            // nothing to do
        }
    }
}