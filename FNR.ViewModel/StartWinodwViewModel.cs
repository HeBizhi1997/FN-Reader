using System.Windows;
using System.Windows.Controls;
using FNR.Common;
using Prism.Commands;
using Prism.Mvvm;

namespace FNR.ViewModel
{
    public class StartWinodwViewModel : BindableBase
    {
        private static readonly string LoginName = "admin";
        private static readonly string LoginPass = "123";

        public DelegateCommand WinCloseCommand { get; set; } = new DelegateCommand(() => Application.Current.Shutdown());
        public DelegateCommand WinMaximizeCommand { get; set; } = new DelegateCommand(() => Application.Current.MainWindow.WindowState = Application.Current.MainWindow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized);
        public DelegateCommand WinMinimizeCommand { get; set; } = new DelegateCommand(() => SystemCommands.MinimizeWindow(Application.Current.MainWindow));
        public DelegateCommand WinMoveCommand { get; set; } = new DelegateCommand(() => Application.Current.MainWindow.DragMove());
        public DelegateCommand<object> LoginCommand { get; set; }

        public DelegateCommand<object> SelectItemChangedCommand { get; set; }

        private ApplicationPages currentPage = ApplicationPages.NovelSearch;
        public ApplicationPages CurrentPage
        {
            get { return currentPage; }
            set { SetProperty(ref currentPage, value); }
        }

        private bool isAdminLogin = false;
        public bool IsAdminLogin
        {
            get { return isAdminLogin; }
            set { SetProperty(ref isAdminLogin, value); }
        }

        public StartWinodwViewModel()
        {
            SelectItemChangedCommand = new DelegateCommand<object>((p) =>
           {
               if (p is ListView listView)
               {
                   CurrentPage = (ApplicationPages)listView.SelectedIndex;
               }
           });

            LoginCommand = new DelegateCommand<object>((p) =>
            {
                if (p is StackPanel ctrl)
                {
                    TextBox user = new TextBox();
                    PasswordBox pass = new PasswordBox();
                    foreach (var child in ctrl.Children)
                    {
                        if (child is TextBox)
                            user = child as TextBox;
                        if (child is PasswordBox)
                            pass = child as PasswordBox;
                    }

                    if (user.Text.Trim() == LoginName && pass.Password.Trim() == LoginPass)
                    {
                        MessageBox.Show("登陆成功！");

                        IsAdminLogin = true;
                    }
                    else
                        MessageBox.Show("登陆失败！");

                    user.Text = string.Empty;
                    pass.Password = string.Empty;
                }

            });
        }
    }
}
