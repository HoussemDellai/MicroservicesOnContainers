using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace HealthChecks
{
    public class SqlConnectionHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;

        public SqlConnectionHealthCheck(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var sqlConnection = new SqlConnection(_connectionString);
                await sqlConnection.OpenAsync();
            }
            catch (SqlException e)
            {
                return HealthCheckResult.Unhealthy("Exception", e);
            }

            return HealthCheckResult.Healthy();
        }
    }
}
