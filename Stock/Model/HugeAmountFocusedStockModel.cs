using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Model
{
    public class HugeAmountFocusedStockModel
    {
        public byte[] UniqueHashCode { get; set; }

        public  string StockCode { get; set; }

        public string StockName { get; set; }

        public decimal AveragePrice { get; set; }

        public DateTime GatherMinute { get; set; }

    }
}
