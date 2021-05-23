using System.Configuration;
using System.Data;

namespace GatherStockData
{
    public class SqlDb
    {
        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Stock"].ToString();
        }

        public static IDbConnection UpdateConnection => new SqlServerConnection(GetConnectionString());

    }
}
