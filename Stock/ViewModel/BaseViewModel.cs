using Stock.Model;
using Stock.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Stock.ViewModel
{
    public class BaseViewModel:NotificationObject
    {
        public UserCacheModel UserCache { get; set; } = MemoryCache.Default["UserCache"] as UserCacheModel;

        public string HostName { get; set; } = Dns.GetHostName();

        public CommonService CommonService { get; set; } = new CommonService();

        public ExportService ExportService { get; set; } = new ExportService();

        private HostConfigModel hostConfig;

        public HostConfigModel HostConfig
        {
            get { return hostConfig; }
            set
            {
                hostConfig = value;
                this.RaisePropertyChanged(nameof(HostConfig));
            }
        }
    }
}
