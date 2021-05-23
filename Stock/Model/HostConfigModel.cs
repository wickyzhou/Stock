using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Model
{
   public class HostConfigModel
    {
		public int Id { get; set; }

		public int TypeId { get; set; }

		public string TypeDesciption { get; set; }

		public string HostName { get; set; }

		public string HostValue { get; set; }

		public int UserId { get; set; }
	}
}
