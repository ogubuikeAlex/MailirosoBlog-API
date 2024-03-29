using MalirosoBlog.Services.Infrastructure;
using Microsoft.Data.SqlClient;

namespace MalirosoBlog.API.Extensions
{
    public static class ConnectionStringBuilder
    {
        public static string BuildConnectionString(this DatabaseConfiguration dbConfig, string partialConnectionString)
        {
            if (string.IsNullOrWhiteSpace(partialConnectionString))
            {
                return null;
            }

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(partialConnectionString)
            {
                DataSource = dbConfig.Host,
                InitialCatalog = dbConfig.Name                
            };

            return builder.ToString();
        }
    }
}
