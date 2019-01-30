using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace FN_Reader.Views.Pages
{
    /// <summary>
    /// Page1.xaml 的交互逻辑
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void NameClearBtn_Click(object sender, RoutedEventArgs e)
        {
            NameTextBox.Text = string.Empty;
        }

        private void PassClearBtn_Click(object sender, RoutedEventArgs e)
        {
            PasswordBox.Password = string.Empty;
        }

        private void PageCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as Index).CurrentPage = Domain.ApplicationPages.None;

            if (Application.Current.MainWindow.FindChild<ColorZone>("_HeadMenu") is ColorZone _headMenu)
            {
                _headMenu.Visibility = Visibility.Visible;
            }
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text != string.Empty && PasswordBox.Password != string.Empty)
            {
                (Application.Current.MainWindow as Index).User = "欢迎登陆 <" + NameTextBox.Text + ">";

                (Application.Current.MainWindow as Index).CurrentPage = Domain.ApplicationPages.None;

                if (Application.Current.MainWindow.FindChild<ColorZone>("_HeadMenu") is ColorZone _headMenu)
                {
                    _headMenu.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
