using Mockingbird.Invocation;
using System.Collections;
using System.Data;
using System.Data.Common;

namespace Mockingbird.Factory.Database
{
    internal class MockedDbCommand : DbCommand
    {
        private readonly DbParameterCollection parameters = new MockedDataParameterCollection();
        private readonly TypeInvocationInfo? typeSetup;
        private readonly ITypeInvocationProvider typeInvocationProvider;

        public MockedDbCommand(TypeInvocationInfo? typeSetup, ITypeInvocationProvider typeInvocationProvider)
        {
            this.typeSetup = typeSetup;
            this.typeInvocationProvider = typeInvocationProvider;
        }

        public override string CommandText { get; set; }
        public override int CommandTimeout { get; set; }
        public override CommandType CommandType { get; set; }
        public override bool DesignTimeVisible { get; set; }
        public override UpdateRowSource UpdatedRowSource { get; set; }
        protected override DbConnection? DbConnection { get; set; }

        protected override DbParameterCollection DbParameterCollection => parameters;

        protected override DbTransaction? DbTransaction { get; set; }

        public override void Cancel()
        {
            // nothing to do
        }

        public override int ExecuteNonQuery()
        {
            string invocationName = CommandText;
            object arguments = Parameters.Cast<IDataParameter>().ToDictionary(parameter => parameter.ParameterName, parameter => parameter.Value);
            InvocationInfo? setupInvocationInfo = typeSetup?.GetInvocation(invocationName, arguments);
            int result = (setupInvocationInfo?.Result as int?) ?? 0;
            typeInvocationProvider.AddInvocation(invocationName, arguments, result);
            return result;
        }

        public override object? ExecuteScalar()
        {
            string invocationName = CommandText;
            object arguments = Parameters.Cast<IDataParameter>().ToDictionary(parameter => parameter.ParameterName, parameter => parameter.Value);
            InvocationInfo? setupInvocationInfo = typeSetup?.GetInvocation(invocationName, arguments);
            typeInvocationProvider.AddInvocation(invocationName, arguments, setupInvocationInfo?.Result);
            return setupInvocationInfo?.Result;
        }

        public override void Prepare()
        {
            // nothing to do
        }

        protected override DbParameter CreateDbParameter()
            => new MockedDbParameter();

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            string invocationName = CommandText;
            object arguments = Parameters.Cast<IDataParameter>().ToDictionary(parameter => parameter.ParameterName, parameter => parameter.Value);
            InvocationInfo? setupInvocationInfo = typeSetup?.GetInvocation(invocationName, arguments);
            IEnumerable? result = setupInvocationInfo?.Result as IEnumerable;
            typeInvocationProvider.AddInvocation(invocationName, arguments, result);
            return new MockedDataReader(result);
        }
    }
}