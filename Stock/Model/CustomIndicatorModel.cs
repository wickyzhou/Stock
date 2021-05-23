using NPOI.Util;
using Stock.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Model
{
    public class CustomIndicatorModel:NotificationObject
    {
		private bool isChecked;

		public bool IsChecked
		{
			get { return isChecked; }
			set
			{
				isChecked = value;
				this.RaisePropertyChanged(nameof(IsChecked));
			}
		}

		public long Id { get; set; }


		private string indicatorDesc;

		public string IndicatorDesc
		{
			get { return indicatorDesc; }
			set
			{
				indicatorDesc = value;
				this.RaisePropertyChanged(nameof(IndicatorDesc));
			}
		}



		private string sqlText;

		public string SqlText
		{
			get{ return sqlText; }
			set
			{
				sqlText = value;
				this.RaisePropertyChanged(nameof(SqlText));
			}
		}
    }
}
