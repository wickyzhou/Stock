using Stock.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.ViewModel.InfoWindow.DataAnalysis
{
   public class CustomIndicatorCreateViewModel:NewDialogViewModel<CustomIndicatorModel>
    {
        public override void Save(object obj)
        {
            base.Save(obj);
        }
    }
}
