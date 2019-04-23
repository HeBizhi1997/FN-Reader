using FNR.ViewModel;
using System.Windows;

namespace FNR.View
{
    /// <summary>
    /// ReadWnd.xaml 的交互逻辑
    /// </summary>
    public partial class ReadWnd : Window
    {
        public ReadWnd()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            (DataContext as ReadWndViewModel).Datadownload();
        }
    }
}
