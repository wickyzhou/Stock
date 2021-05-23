using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Model
{
    public class StockBaseModel
    {
        public byte[] UniqueHashCode { get; set; }

        public string StockCode { get; set; }

        public string StockName { get; set; }

        public string ShortName { get; set; }

    }
}
