using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Mockingbird.Tests.Services
{

    internal class DbConnectionService
    {
        private readonly IDbConnection dbConnection;

        public DbConnectionService(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        internal async Task<IEnumerable<DbConnectionItem>> QueryAsync(string input, CancellationToken cancellationToken)
        {
            return await dbConnection.QueryAsync<DbConnectionItem>("SELECT * FROM dbo.FooBar WHERE text = @input", new { input });
        }

        internal async Task<long> ExecuteScalarAsync(string input, CancellationToken cancellationToken)
        {
            return await dbConnection.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM dbo.FooBar WHERE text = @input", new { input });
        }
    }
}