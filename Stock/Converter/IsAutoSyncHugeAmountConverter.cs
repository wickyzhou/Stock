using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Converter
{
    public class IsAutoSyncHugeAmountConverter : BaseValueConverter<IsAutoSyncHugeAmountConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "自动更新";
            return (bool)value ? "停止更新":"自动更新"  ;
        }
    }
}
