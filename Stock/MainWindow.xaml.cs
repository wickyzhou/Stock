using Model;
using Stock.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Stock
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        IEnumerable<MainMenuModel> menuModels;
        private int currentMenuId = 0;
        private string defaultUri;
        private readonly UserAccountModel _user = new UserAccountModel { SuperAdmin =true,Id=1};
        public MainWindow()
        {
            InitializeComponent();
            this.MainFrame.Height= SystemParameters.PrimaryScreenHeight - 150;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //动态加载列表以及获取默认页面
            GetListBoxItems(_user.SuperAdmin, _user.Id);
        }

        private void GetListBoxItems(bool superAdmin, int userId)
        {
            // 获取用户默认界面
            GetUserDefaultPage(userId);

            // 获取列表
            menuModels = new MainWindowService().GetUserMenu(superAdmin, userId);
            foreach (var item in menuModels)
            {
                StackPanel sp = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                    ,
                    VerticalAlignment = VerticalAlignment.Center
                    ,
                    Name = "sp" + item.ID.ToString()
                };

                TextBlock tb1 = new TextBlock();
                tb1.Text = HttpUtility.HtmlDecode(item.TB1Text);
                tb1.FontSize = item.TB1FontSize;
                tb1.FontFamily = FindResource("Iconfont") as FontFamily;
                tb1.Margin = new Thickness(item.TB1MarginLeft, item.TB1MarginTop, item.TB1MarginRight, item.TB1MarginBottom);
                tb1.VerticalAlignment = VerticalAlignment.Center;


                TextBlock tb2 = new TextBlock
                {
                    Text = item.TB2Text,
                    FontFamily = new FontFamily(item.TB2FontFamily),
                    Margin = new Thickness(item.TB2MarginLeft, item.TB2MarginTop, item.TB2MarginRight, item.TB2MarginBottom),
                    VerticalAlignment = VerticalAlignment.Center
                };
                sp.Children.Add(tb1);
                sp.Children.Add(tb2);
                sp.ContextMenu = new ContextMenu();
                this.MenuListBox.Items.Add(sp);
            }
        }

        private void MenuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuListBox.SelectedItems.Count == 1)
            {
                var menuId = int.Parse((MenuListBox.SelectedItem as StackPanel).Name.Replace("sp", ""));
                currentMenuId = menuId;
                var uri = menuModels.FirstOrDefault(m => m.ID == menuId).Uri;
                this.MainFrame.Content = null;
                this.MainFrame.Navigate(new Uri(uri, UriKind.Relative));
            }
        }

        private void TbClose_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            new UserService().RecordLogoutLog(_user.Id);
        }

        private void TbMinimize_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void GetUserDefaultPage(int userId)
        {

            string url = new UserInformationService().GetDefaultPage(userId);
            if (!string.IsNullOrEmpty(url))
            {
                defaultUri = url;
                this.MainFrame.Navigate(new Uri(url, UriKind.Relative));
            }
            else
            {
                this.MainFrame.Navigate(new Uri("/View/IndexPage/UserDefaultPage.xaml", UriKind.Relative));
            }

        }

        private void BtnShowDefaultPage_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(defaultUri))
            {
                this.MenuListBox.SelectedItem = null;
                this.MainFrame.Navigate(new Uri(defaultUri, UriKind.Relative));
            }
            else
            {
                this.MenuListBox.SelectedItem = null;
                this.MainFrame.Navigate(new Uri("/View/IndexPage/UserDefaultPage.xaml", UriKind.Relative));
            }
        }

        private void HlAccountManager_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnModifyPassword_Click(object sender, RoutedEventArgs e)
        {

            Regex reg = new Regex("^[0-9]{1,20}$");
            if (!reg.IsMatch(this.TbPassword.Password))
            {
                MessageBox.Show("密码只能为1-20位数字");
                return;
            }


            new UserService().ModifyUserPassword(_user.Id, this.TbPassword.Password);
            MessageBox.Show($"修改成功，新密码为【{this.TbPassword.Password}】");
        }
    }
}
