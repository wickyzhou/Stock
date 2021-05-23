using Dapper;
using Stock.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock.Service
{
   public class UserInformationService
    {

        public string GetDefaultPage(int userId)
        {
            string sql = @"  SELECT DefaultPage FROM UserInfomation where UserId = @UserId;";
            using (var connection = SqlDb.UpdateConnection)
            {
                return Convert.ToString(connection.ExecuteScalar(sql, new { UserId = userId }));
            }
        }

 
    }
}
