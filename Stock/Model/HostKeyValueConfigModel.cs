using Stock.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Stock.Model
{
   public class HostKeyValueConfigModel:NotificationObject
    {
        public int Id { get; set; }

        public int TypeId { get; set; }

        public string TypeDesciption { get; set; }

        public int UserId { get; set; }

        public string HostName { get; set; }

		private string inputValue;

		public string InputValue
		{
			get { return inputValue; }
			set
			{
				inputValue  = value;
				this.RaisePropertyChanged(nameof(InputValue));
			}
		}

	}
}
