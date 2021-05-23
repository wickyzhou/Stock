using Dapper;
using Stock.Common;
using Stock.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Service
{
    public class GatherMasterService
    {
        public HostKeyValueConfigModel GetExePath(int userId, string hostName, int typeId)
        {
            string sql = @" select * from HostKeyValueConfig where UserId=@UserId and HostName=@HostName and TypeId=@TypeId;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<HostKeyValueConfigModel>(sql, new { UserId = userId, HostName = hostName, TypeId = typeId }).FirstOrDefault();
            }
        }

        public int SaveConfig(HostKeyValueConfigModel model)
        {
            string sql = string.Empty;
            if (model.Id == 0)
                sql = @" insert into  HostKeyValueConfig(TypeId,TypeDesciption,UserId,HostName,InputValue) values(@TypeId,@TypeDesciption,@UserId,@HostName,@InputValue)  ; set @IdReturn = SCOPE_IDENTITY();";
            else 
                sql = @" update HostKeyValueConfig set InputValue=@InputValue where Id=@Id;  set @IdReturn=@Id; ";

            using (var connection = SqlDb.UpdateConnection)
            {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@IdReturn", DbType.Int32, direction: ParameterDirection.Output);
                dp.Add("@Id", model.Id, DbType.Int32, ParameterDirection.Input);
                dp.Add("@UserId", model.UserId, DbType.Int32, ParameterDirection.Input);
                dp.Add("@HostName", model.HostName, DbType.String, ParameterDirection.Input);
                dp.Add("@InputValue", model.InputValue, DbType.String, ParameterDirection.Input);
                dp.Add("@TypeId", model.TypeId, DbType.Int32, ParameterDirection.Input);
                dp.Add("@TypeDesciption", model.TypeDesciption, DbType.String, ParameterDirection.Input);
                connection.Execute(sql, dp);

                return dp.Get<int>("@IdReturn");
            }
        }
    }
}
