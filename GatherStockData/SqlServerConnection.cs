using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GatherStockData
{
    public class SqlServerConnection : IDbConnection
    {
        public SqlConnection Connection { get; set; }
        public SqlServerConnection(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
        }

        public string ConnectionString
        {
            get { return Connection.ConnectionString; }
            set { Connection.ConnectionString = value; }
        }

        public int ConnectionTimeout => Connection.ConnectionTimeout;

        public string Database => Connection.Database;

        public ConnectionState State => Connection.State;

        public IDbTransaction BeginTransaction()
        {
            return Connection.BeginTransaction();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return Connection.BeginTransaction(il);
        }

        public void Close()
        {
            Connection.Close();
        }

        public void ChangeDatabase(string databaseName)
        {
            Connection.ChangeDatabase(databaseName);
        }

        public IDbCommand CreateCommand()
        {
            return Connection.CreateCommand();
        }

        public void Open()
        {
            Connection.Open();
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }
    }
}
