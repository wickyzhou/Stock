using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace GatherStockData
{
    public static class SqlHelper
    {
        static readonly string constr = ConfigurationManager.ConnectionStrings["Stock"].ToString();

        public static DataTable ExecuteDataTable(string sql, params SqlParameter[] paras)
        {
            DataTable table;
            using (SqlConnection connection = new SqlConnection(constr))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    if (paras != null)
                        command.Parameters.AddRange(paras);
                    DataSet dataSet = new DataSet();
                    using (SqlDataAdapter sda = new SqlDataAdapter(command))
                    {
                        sda.Fill(dataSet);
                    }
                    //new SqlDataAdapter(command).Fill(dataSet);
                    table = dataSet.Tables[0];
                }
            }
            return table;
        }

        public static DataTable ExecuteDataTableProcedure(string procedureName, params SqlParameter[] paras)
        {
            DataTable table;

            using (SqlConnection connection = new SqlConnection(constr))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = procedureName;
                    command.CommandType = CommandType.StoredProcedure;
                    if (paras != null)
                        command.Parameters.AddRange(paras);
                    DataSet dataSet = new DataSet();
                    using (SqlDataAdapter sda = new SqlDataAdapter(command))
                    {
                        sda.Fill(dataSet);
                    }
                    //new SqlDataAdapter(command).Fill(dataSet);
                    table = dataSet.Tables[0];
                }
            }
            return table;
        }


        public static int ExecuteNonQuery(string sql, params SqlParameter[] paras)
        {
            int num;
            using (SqlConnection connection = new SqlConnection(constr))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    if (paras != null)
                        command.Parameters.AddRange(paras);
                    num = command.ExecuteNonQuery();
                }
            }
            return num;
        }

        public static int ExecuteNonQueryProcedure(string procedureName, params SqlParameter[] paras)
        {
            int num;
            using (SqlConnection connection = new SqlConnection(constr))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = procedureName;
                    command.CommandType = CommandType.StoredProcedure;
                    if (paras != null)
                        command.Parameters.AddRange(paras);
                    num = command.ExecuteNonQuery();
                }
            }
            return num;
        }

        public static object ExecuteScalar(string sql, params SqlParameter[] paras)
        {
            object obj2;
            using (SqlConnection connection = new SqlConnection(constr))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    if (paras != null)
                        command.Parameters.AddRange(paras);
                    obj2 = command.ExecuteScalar();
                }
            }
            return obj2;
        }

        public static object ExecuteScalarProcedure(string procedureName, params SqlParameter[] paras)
        {
            object obj2;
            using (SqlConnection connection = new SqlConnection(constr))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = procedureName;
                    command.CommandType = CommandType.StoredProcedure;
                    if (paras != null)
                        command.Parameters.AddRange(paras);
                    obj2 = command.ExecuteScalar();
                }
            }
            return obj2;
        }


        public static string Obj2String(object value)
        {
            if (value == DBNull.Value || value == null)
                return "";
            else return (string)value;
        }

        public static DateTime Obj2DateTime(object value)
        {
            if (value == DBNull.Value || value == null)
                return new DateTime(1970, 1, 1);// "";
            else return (DateTime)value;
        }

        public static object FromDbValue(object value)
        {
            if (value == DBNull.Value)
            {
                return null;
            }
            return value;
        }

        public static object ToDbValue(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            return value;
        }

        public static List<T> DataTableToModelList<T>(DataTable table)
        {
            List<T> list = new List<T>();
            T t = default(T);
            PropertyInfo[] propertypes = null;
            string tempName = string.Empty;
            foreach (DataRow row in table.Rows)
            {
                t = Activator.CreateInstance<T>();
                propertypes = t.GetType().GetProperties();
                foreach (PropertyInfo pro in propertypes)
                {
                    tempName = pro.Name;
                    if (table.Columns.Contains(tempName))
                    {
                        object value = row[tempName];
                        if (value.GetType() == typeof(System.DBNull))
                        {
                            value = null;
                        }
                        pro.SetValue(t, value, null);
                    }
                }
                list.Add(t);
            }
            return list;
        }

        public static void BulkInsert(DataTable dt, string modeltable)
        {
            if (dt.Rows.Count > 0)
            {
                using (SqlBulkCopy bkc = new SqlBulkCopy(constr))
                {
                    bkc.DestinationTableName = modeltable;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        bkc.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                    }
                    bkc.WriteToServer(dt);
                }
            }

        }
    }
}
