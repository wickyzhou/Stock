using Microsoft.Win32;
using Stock.Command;
using Stock.Model;
using Stock.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Windows.Forms;
using System.Windows.Media;

namespace Stock.ViewModel.IndexPage
{
    public class GatherMasterViewModel : NotificationObject
    {
        private static readonly GatherMasterService _gatherMasterService = new GatherMasterService();
        private static readonly int _userId = 0;
        private static readonly string _hostName = Dns.GetHostName();

       public GatherMasterViewModel()
        {
            InitDataProperty();
            InitCommandProperty();
        }



        private void InitDataProperty()
        {
            PathConfig = _gatherMasterService.GetExePath(_userId, _hostName,1)??new HostKeyValueConfigModel() { UserId= _userId ,HostName= _hostName ,TypeId= 1,Id=0,TypeDesciption="采集程序路径"};
        }

        private void InitCommandProperty()
        {
            GatherMasterRunExeCommand = new DelegateCommand(RunExe);
            GatherMasterSelectExePathCommand = new DelegateCommand(SelectExePath);
        }

        private void SelectExePath(object obj)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            //dialog.Multiselect = true;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件夹";
            dialog.Filter = "所有文件(*.*)|*.exe";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                PathConfig.InputValue = dialog.FileName;
                int id = _gatherMasterService.SaveConfig(PathConfig);
                PathConfig.Id = id;
            }
        }

        #region 数据属性
        private SolidColorBrush backgroundColor;

        public SolidColorBrush BackgroundColor
        {
            get { return backgroundColor; }
            set
            {
                backgroundColor = value;
                this.RaisePropertyChanged(nameof(BackgroundColor));
            }
        }

        private HostKeyValueConfigModel pathConfig;

        public HostKeyValueConfigModel PathConfig
        {
            get { return pathConfig; }
            set
            {
                pathConfig = value;
                this.RaisePropertyChanged(nameof(PathConfig));
            }
        }

        #endregion



        #region 命令委托
        public DelegateCommand GatherMasterSelectExePathCommand { get; set; }
        public DelegateCommand GatherMasterRunExeCommand { get; set; }

        #endregion

        #region 命令实现
        private void RunExe(object obj)
        {
            //Type type = typeof(Colors);
            //var infos = type.GetProperties();
            //double c = Math.Sqrt(infos.Count());
            //int rowCount = (int)(Math.Floor(c));
            //int colCount = rowCount + (int)Math.Ceiling(c - (double)rowCount);

            //for (int i = 0; i < infos.Length; i++)
            //{
            //    var info = infos[i].Name;
            //    Thread.Sleep(1000);
            //    BackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(info));
            //}
        }
        #endregion





    }
}
