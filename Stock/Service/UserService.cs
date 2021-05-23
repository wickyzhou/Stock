using Dapper;
using Model;
using Stock.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock.Service
{
    public class UserService
    {

        public IList<UserAccountModel> GetAllUsers()
        {
            string sql = @"SELECT * FROM  UserAccount ;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<UserAccountModel>(sql).ToList();
            }
        }

        public bool RecordLoginLog(int id)
        {
            string sql = @" Insert into UserLoginLog(UserId,LoginTime,LogoutTime) Values(@Id,default,null) ";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, new { Id = id }) > 0;
            }
        }


        public bool RecordLogoutLog(int id)
        {
            string sql = @" Insert into UserLoginLog(UserId,LoginTime,LogoutTime) Values(@Id,NULL,default)";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, new { Id = id }) > 0;
            }
        }


        public bool ModifyUserPassword(int id,string pwd)
        {
            string sql = @" update User set Password =@Password  where Id=@Id";
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Execute(sql, new { Id = id ,Password=pwd}) > 0;
            }
        }

    }
}
