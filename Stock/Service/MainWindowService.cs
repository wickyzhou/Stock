using Dapper;
using Model;
using Stock.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Service
{
    public class MainWindowService
    {
        public IList<MainMenuModel> GetUserMenu(bool superAdmin, int userId)
        {
            string sql;
            if (superAdmin)
                sql = @"SELECT * FROM MainMenu";
            else
                sql = @" SELECT a.* FROM MainMenu a join SJPageOwner b on a.ID=b.MainMenuId where UserId=@UserId
                    union
                   SELECT a.* FROM MainMenu a join  SJPageOwner b on a.ID = b.MainMenuId join  UserRole c on b.RoleId = c.RoleId where c.UserId = @UserId";

             
            using (var connection = SqlDb.UpdateConnection)
            {
                return connection.Query<MainMenuModel>(sql,new { UserId=userId}).ToList();
            }
        }


  
    }
}
