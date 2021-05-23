using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class UserAccountModel
    {
        public int Id { get; set; }

        public string LoginName { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }

        public int OrgId { get; set; }

        public bool SuperAdmin { get; set; }

    }
}
