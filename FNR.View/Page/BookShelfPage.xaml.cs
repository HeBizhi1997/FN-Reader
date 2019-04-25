using FNR.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace FNR.View
{
    /// <summary>
    /// BookShelfPage.xaml 的交互逻辑
    /// </summary>
    public partial class BookShelfPage : Page
    {
        public BookShelfPage()
        {
            InitializeComponent();
        }

        private void ItemsControl_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button btn)
            {
                ReadWnd readWnd = new ReadWnd()
                {
                    DataContext = new ReadWndViewModel(btn.Tag)
                };
                readWnd.Show();
                Application.Current.MainWindow.Hide();
            }
        }
    }
}
