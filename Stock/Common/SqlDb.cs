using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace Stock.Common
{
    public class SqlDb
    {
        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Connection"].ToString();
        }

        public static IDbConnection UpdateConnection => new SqlServerConnection(GetConnectionString());

    }
}
