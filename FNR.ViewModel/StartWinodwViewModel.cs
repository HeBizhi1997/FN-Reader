using System.Windows;
using System.Windows.Controls;
using FNR.Common;
using Prism.Commands;
using Prism.Mvvm;

namespace FNR.ViewModel
{
    public class StartWinodwViewModel : BindableBase
    {
        public DelegateCommand WinCloseCommand { get; set; } = new DelegateCommand(() => Application.Current.Shutdown());
        public DelegateCommand WinMaximizeCommand { get; set; } = new DelegateCommand(() => Application.Current.MainWindow.WindowState = Application.Current.MainWindow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized);
        public DelegateCommand WinMinimizeCommand { get; set; } = new DelegateCommand(() => SystemCommands.MinimizeWindow(Application.Current.MainWindow));
        public DelegateCommand WinMoveCommand { get; set; } = new DelegateCommand(() => Application.Current.MainWindow.DragMove());
        public DelegateCommand<object> LoginCommand { get; set; }
        public DelegateCommand RegisterCommand { get; set; }

        public DelegateCommand<object> SelectItemChangedCommand { get; set; }

        private ApplicationPages currentPage = ApplicationPages.NovelSearch;
        public ApplicationPages CurrentPage
        {
            get { return currentPage; }
            set { SetProperty(ref currentPage, value); }
        }

        private int userLevel = -1;
        public int UserLevel
        {
            get { return userLevel; }
            set { SetProperty(ref userLevel, value); }
        }

        private string userName = "访客";
        public string UserName
        {
            get { return userName; }
            set { SetProperty(ref userName, value); }
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

                    foreach (var item in ElasticSearch.ElasticHelper.QueryUser(user.Text.Trim()))
                    {
                        if (user.Text.Trim() == item.Name && pass.Password.Trim() == item.Password)
                        {
                            MessageBox.Show("登陆成功！");
                            if (item.Level > 0)
                            {
                                UserLevel = 1;
                                UserName = "管理员";
                            }
                            else
                            {
                                UserName = item.Name;
                            }
                            user.Text = string.Empty;
                            pass.Password = string.Empty;
                            return;
                        }
                    }

                    MessageBox.Show("登录失败!!!");
                    user.Text = string.Empty;
                    pass.Password = string.Empty;
                }

            });

            RegisterCommand = new DelegateCommand(() => CurrentPage = ApplicationPages.Register);
        }
    }
}
