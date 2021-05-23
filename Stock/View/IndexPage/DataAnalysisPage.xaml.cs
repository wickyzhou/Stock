using Stock.ViewModel.IndexPage;
using System.Windows.Controls;

namespace Stock.View.IndexPage
{
    /// <summary>
    /// DataAnalysisPage.xaml 的交互逻辑
    /// </summary>
    public partial class DataAnalysisPage : Page
    {
        public DataAnalysisPage()
        {   
            InitializeComponent();
            this.DataContext = new DataAnalysisPageViewModel();
        }
    }
}
