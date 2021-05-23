using Stock.ViewModel.InfoWindow.DataAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Stock.View.InfoWindow.DataAnalysis
{
    /// <summary>
    /// CustomIndicatorCreateView.xaml 的交互逻辑
    /// </summary>
    public partial class CustomIndicatorCreateView : Window
    {
        public CustomIndicatorCreateView()
        {
            InitializeComponent();
            this.DataContext = new CustomIndicatorCreateViewModel();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (send, e) => { this.Close(); }));
            this.MouseLeftButtonDown += (sender, e) => { this.DragMove(); };
        }
    }
}
