using Dapper;
using Stock.Common;
using Stock.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Service
{
    public class CustomIndicatorService : IRepository<CustomIndicatorModel>
    {

        public IEnumerable<CustomIndicatorModel> GetLists()
        {
            string sql = @" select * from CustomIndicator ;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<CustomIndicatorModel>(sql);
            }
        }

        public bool Delete(CustomIndicatorModel entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(IEnumerable<CustomIndicatorModel> entities)
        {
            throw new NotImplementedException();
        }

        public CustomIndicatorModel GetById(object id)
        {
            string sql = @" select * from CustomIndicator where Id=@Id;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<CustomIndicatorModel>(sql, new { Id=(int)id}).FirstOrDefault();
            }
        }

        public bool Insert(CustomIndicatorModel entity)
        {
            string sql = @" insert into CustomIndicator(Id,IndicatorDesc,SqlText) select  case when (select max(Id) from CustomIndicator) is null then 1 else (select max(Id) from CustomIndicator)*2 end as Id,@IndicatorDesc,@SqlText";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql,entity) > 0;
            }
        }

        public bool Insert(IEnumerable<CustomIndicatorModel> entities)
        {
            throw new NotImplementedException();
        }

        public bool Update(CustomIndicatorModel entity)
        {
            string sql = @" update CustomIndicator set IndicatorDesc = @IndicatorDesc,SqlText = @SqlText where Id = @Id;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, entity) > 0;
            }
        }

        public int Update(IEnumerable<CustomIndicatorModel> entities)
        {
            throw new NotImplementedException();
        }

        public int GetContinuousId()
        {
            int comparison = 1;
            List<int> Ids = new List<int>();
            string sql = @" select Id from  CustomIndicator order by Id;";
            using (var connection = SqlDb.UpdateConnection)
            {
                Ids= connection.Query<int>(sql).ToList();
            }
            if (Ids.Count == 0)
                return 1;
            else
            {
                foreach (int id in Ids)
                {
                    if ((id & comparison) == 0)
                        break;
                        comparison *= 2;
                }
                return comparison;
            }
        }
    }
}
