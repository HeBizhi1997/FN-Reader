using FNR.Component;
using FNR.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FNR.View
{
    /// <summary>
    /// SearchPage.xaml 的交互逻辑
    /// </summary>
    public partial class SearchPage : Page
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        private void FrameworkElement_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as FrameworkElement).ToolTip = null;
            (sender as FrameworkElement).ToolTip = new BookCard()
            {
                DataContext = (sender as FrameworkElement).DataContext
            };
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

        private void ItemsControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is TextBlock tblk)
            {

            }
        }
    }
}
