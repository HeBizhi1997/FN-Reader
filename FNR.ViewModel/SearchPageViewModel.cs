using Prism.Mvvm;
using Prism.Commands;
using System.Windows.Controls;
using FNR.DataStructure;
using System.Collections.Generic;
using FNR.Component;

namespace FNR.ViewModel
{
    public class SearchPageViewModel : BindableBase
    {
        public DelegateCommand<string> QueryCommand { get; set; }

        public SearchPageViewModel()
        {
            QueryCommand = new DelegateCommand<string>((p) =>
            {
                NovelList = ElasticSearch.ElasticHelper.Query(p, 5);
                //foreach (var item in ElasticSearch.ElasticHelper.Query(p, 3))
                //{
                //    NovelList.Add(new BookCard(item));
                //}
            });
        }

        private List<Novel> novelList = new List<Novel>();
        public List<Novel> NovelList
        {
            get { return novelList; }
            set { SetProperty(ref novelList, value); }
        }
    }
}
