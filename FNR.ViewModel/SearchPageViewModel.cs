using Prism.Mvvm;
using Prism.Commands;
using FNR.Model;
using System.Collections.Generic;
using System.Windows.Controls;

namespace FNR.ViewModel
{
    public class SearchPageViewModel : BindableBase
    {
        public DelegateCommand<string> QueryCommand { get; set; }
        public SearchPageViewModel()
        {
            QueryCommand = new DelegateCommand<string>((p) =>
            {
                NovelList = ElasticSearch.ElasticHelper.Query(p, 10);
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
