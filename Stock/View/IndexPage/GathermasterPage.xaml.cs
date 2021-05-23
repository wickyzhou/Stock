using Stock.ViewModel.IndexPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Stock.View.IndexPage
{
    /// <summary>
    /// GathermasterPage.xaml 的交互逻辑
    /// </summary>
    public partial class GathermasterPage : Page
    {
        public GathermasterPage()
        {
            InitializeComponent();
            this.DataContext = new GatherMasterViewModel();
            InitData();
        }

        private void InitData()
        {
            Type type = typeof(Colors);
            var infos = type.GetProperties();
            double c = Math.Sqrt(infos.Count());
            int colCount = (int)(Math.Floor(c));
            int rowCount = colCount + (int)Math.Ceiling(c - (double)colCount);

            ScrollViewer scrollViewer = new ScrollViewer();
            WrapPanel wrapPanel = new WrapPanel();
            
            //wrapPanel.Background = new SolidColorBrush(Colors.Red);
            // 定义列
            for (int i = 0; i < infos.Length; i++)
            {
                var info = infos[i].Name;
                Grid grid = new Grid();
                grid.Height = 100;
                grid.Width = 100;
                grid.Background= new SolidColorBrush((Color)ColorConverter.ConvertFromString(info));

                TextBox  textBox = new TextBox();
                textBox.TextAlignment = TextAlignment.Center;
                textBox.Text = info;
                textBox.FontSize = 12;
                textBox.VerticalAlignment = VerticalAlignment.Center;
                //textBox.HorizontalAlignment = HorizontalAlignment.Center;
                textBox.TextWrapping = TextWrapping.Wrap;
                textBox.Margin = new Thickness(10,0,10,0);
                textBox.Height = 35;
                grid.Children.Add(textBox);
                wrapPanel.Children.Add(grid);
            }
            scrollViewer.Content = wrapPanel;
            this.ColorGrid.Children.Add(scrollViewer);
        }
    }
}
