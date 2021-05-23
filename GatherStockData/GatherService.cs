using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GatherStockData
{
    public class GatherService
    {
        public List<ItemSourceModel> GetItemSource()
        {
            string sql = @" select * from ItemSourceTable;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ItemSourceModel>(sql).ToList();
            }
        }

        public ItemSourceModel GetItemSource(int id)
        {
            string sql = @" select * from ItemSourceTable where Id = @Id";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<ItemSourceModel>(sql,new { Id = id}).First();
            }
        }
    }
}
