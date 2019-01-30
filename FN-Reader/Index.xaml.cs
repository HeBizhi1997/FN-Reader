using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FN_Reader.Domain;

namespace FN_Reader
{
    /// <summary>
    /// Index.xaml 的交互逻辑
    /// </summary>
    public partial class Index
    {
        public Index()
        {

            this.WindowStyle = WindowStyle.None;
            this.AllowsTransparency = true;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Style = this.FindResource("WinStyle") as Style;
            this.AllowDrop = false;

            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, MaximizeWindow, CanResizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, MinimizeWindow, CanMinimizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, RestoreWindow, CanResizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.ShowSystemMenuCommand, ShowSystemMenu));

        }

        #region DependencyProperty
        //窗体页面
        public ApplicationPages CurrentPage
        {
            get { return (ApplicationPages)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }

        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register("CurrentPage", typeof(ApplicationPages), typeof(Index), new PropertyMetadata(ApplicationPages.None));

        //登录用户名
        public string User
        {
            get { return (string)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }

        public static readonly DependencyProperty UserProperty =
            DependencyProperty.Register("User", typeof(string), typeof(Index), new PropertyMetadata(string.Empty));

        #endregion

        #region rewrite Event

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            if (SizeToContent == SizeToContent.WidthAndHeight)
                InvalidateMeasure();

            //为改变主题按钮添加点击事件
            if (GetTemplateChild("_ThemesBtn") is Button _themesBtn)
                _themesBtn.Click += ShowSystemThemes;

            if (GetTemplateChild("_LoginBtn") is Button _loginBtn)
                _loginBtn.Click += ShowLoginDialog;
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;            
        }

        #endregion

        #region Window Commands

        private void CanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void CanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode != ResizeMode.NoResize;
        }

        private void CloseWindow(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void MaximizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void MinimizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void RestoreWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

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
            if (GetTemplateChild("_LeftFlyout") is Flyout flyout)
                flyout.IsOpen = !flyout.IsOpen;
        }

        private void ShowLoginDialog(object sender, RoutedEventArgs e)
        {
            CurrentPage = ApplicationPages.Login;
            _HeadMenu.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Page Controls Events

        private void SearchClearBtn(object sender, RoutedEventArgs e)
        {
            SearchTbx.Text = string.Empty;
        }

        private void PageSelectChange(object sender, SelectionChangedEventArgs e)
        {
            switch((sender as ListBox).SelectedIndex)
            {
                case 1:
                    CurrentPage = ApplicationPages.BookShelf;
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
