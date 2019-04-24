using Prism.Mvvm;
using Prism.Commands;
using FNR.Model;
using System;
using FNR.ElasticSearch;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FNR.ViewModel
{
    public class RegisterPageViewModel : BindableBase
    {
        public DelegateCommand<object> RegisterCommand { get; set; }
        public DelegateCommand<object> NameVaildCommand { get; set; }

        public RegisterPageViewModel()
        {
            RegisterCommand = new DelegateCommand<object>((p) =>
              {
                  NewUser.Id = int.Parse(DateTime.Now.ToString("ddHHmmss")) * 10 + new Random().Next(0, 9);
                  if (IsFemale)
                      NewUser.Gender = 0;
                  else
                      NewUser.Gender = 1;

                  if (string.IsNullOrEmpty(NewUser.Name) || string.IsNullOrEmpty(NewUser.Password))
                  {
                      MessageBox.Show("用户名密码不可为空!!!");
                      return;
                  }

                  if (!IsNameUnique)
                  {
                      MessageBox.Show("用户名已存在!!");
                      return;
                  }
                  ElasticHelper.CreateUserIndex();
                  ElasticHelper.Insert(NewUser);
                  MessageBox.Show("注册成功!!!");
                  NewUser = new User();
                  var vm = Application.Current.MainWindow.DataContext as StartWinodwViewModel;
                  vm.CurrentPage = Common.ApplicationPages.NovelSearch;
              });

            NameVaildCommand = new DelegateCommand<object>((p) =>
              {
                  foreach (var item in ElasticHelper.QueryUser((p as TextBox).Text))
                  {
                      if (item.Name == (p as TextBox).Text)
                      {
                          IsNameUnique = false;
                          (p as TextBox).Foreground = Brushes.Red;
                      }
                      else
                      {
                          IsNameUnique = true;
                          (p as TextBox).Foreground = Brushes.White;
                      }
                  }
              });
        }

        private User newUser = new User();
        private bool isFemale = false;
        private bool isNameUnique = true;

        public User NewUser
        {
            get { return newUser; }
            set { SetProperty(ref newUser, value); }
        }

        public bool IsFemale
        {
            get { return isFemale; }
            set { SetProperty(ref isFemale, value); }
        }

        public bool IsNameUnique
        {
            get { return isNameUnique; }
            set { SetProperty(ref isNameUnique, value); }
        }

    }
}
