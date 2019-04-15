using System.Windows;
using System.Windows.Input;

namespace FNR.View
{
    /// <summary>
    /// StartWinodw.xaml 的交互逻辑
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        #region Window Commands

        private void ShowSystemMenu(object sender, ExecutedRoutedEventArgs e)
        {
            if (!(e.OriginalSource is FrameworkElement element))
                return;

            var point = WindowState == WindowState.Maximized ? new Point(0, element.ActualHeight)
                : new Point(Left + BorderThickness.Left, element.ActualHeight + Top + BorderThickness.Top);
            point = element.TransformToAncestor(this).Transform(point);
            SystemCommands.ShowSystemMenu(this, point);
        }

        private void ShowSystemThemes(object sender, RoutedEventArgs e)
        {
            LeftThemesChangeFlyout.IsOpen = !LeftThemesChangeFlyout.IsOpen;
        }

        //private void ShowLoginDialog(object sender, RoutedEventArgs e)
        //{
        //    CurrentPage = ApplicationPages.Login;
        //    _HeadMenu.Visibility = Visibility.Collapsed;
        //}

        private void MoveWinodw(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void MaximizeWindow(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }
        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }
        #endregion
    }
}
