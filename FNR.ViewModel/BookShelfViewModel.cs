using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FNR.Model;
using Prism.Commands;
using Prism.Mvvm;

namespace FNR.ViewModel
{
    public class BookShelfViewModel : BindableBase
    {
        private User reader;
        public User Reader
        {
            get { return reader; }
            set { SetProperty(ref reader, value); }
        }

        private List<Novel> novelsList = new List<Novel>();
        public List<Novel> NovelList
        {
            get { return novelsList; }
            set { SetProperty(ref novelsList, value); }
        }
        public BookShelfViewModel()
        {
            reader = (Application.Current.MainWindow.DataContext as StartWinodwViewModel).Reader;

            foreach (var item in Reader.Books)
            {
                foreach (var v in ElasticSearch.ElasticHelper.QueryById(item.BookID))
                    NovelList.Add(v);               
            }
        }

    }
}
