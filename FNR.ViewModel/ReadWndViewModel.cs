using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FNR.Crawler;
using FNR.Model;
using Prism.Commands;
using Prism.Mvvm;

namespace FNR.ViewModel
{
    public class ReadWndViewModel : BindableBase
    {
        private readonly Uri IPNet = new Uri("http://www.shenjidaili.com/open/");//IP代理网站

        private List<string> IPList = new List<string>();//存放获取的代理IP
        private List<string> OriginHtml = new List<string>();//存放原始网页
        private DateTime StrartTime = DateTime.Now; //任务开始时间
        private BackgroundWorker Worker;
        private int links = 0;


        public DelegateCommand<object> WinCloseCommand { get; set; } = new DelegateCommand<object>((p) =>
        {
            (p as Window).Close();
            Application.Current.MainWindow.Show();
        });
        public DelegateCommand<object> WinMaximizeCommand { get; set; } = new DelegateCommand<object>((p) => (p as Window).WindowState = (p as Window).WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized);
        public DelegateCommand<object> WinMinimizeCommand { get; set; } = new DelegateCommand<object>((p) => SystemCommands.MinimizeWindow(p as Window));
        public DelegateCommand<object> WinMoveCommand { get; set; } = new DelegateCommand<object>((p) => (p as Window).DragMove());
        public DelegateCommand<object> SelectItemChangedCommand { get; set; }
        public DelegateCommand DownloadTenSectionsCommand { get; set; }

        public ReadWndViewModel(object data)
        {
            Book = data as Novel;

            GetSectionLinks(Book);

            SelectItemChangedCommand = new DelegateCommand<object>((p) =>
            {
                if (p is ListView listView)
                {
                    var htmlContent = HtmlCrawler.GetHtmlContent(Book.Sections[listView.SelectedIndex].Html);
                    CurrentContent = HtmlAnalysis.AnalysisSectionContent(htmlContent);
                }
            });

            DownloadTenSectionsCommand = new DelegateCommand(() => Datadownload());
        }

        private Novel book;
        private string currentContent;
        private bool isDataDownloading = false;
        private string currentDownloadMessage;
        private double currentDownloadProgress = 0d;



        public Novel Book
        {
            get { return book; }
            set { SetProperty(ref book, value); }
        }
        public string CurrentContent
        {
            get { return currentContent; }
            set { SetProperty(ref currentContent, value); }
        }
        public bool IsDataDownloading
        {
            get { return isDataDownloading; }
            set { SetProperty(ref isDataDownloading, value); }
        }
        public string CurrentDownloadMessage
        {
            get { return currentDownloadMessage; }
            set { SetProperty(ref currentDownloadMessage, value); }
        }
        public double CurrentDownloadProgress
        {
            get { return currentDownloadProgress; }
            set { SetProperty(ref currentDownloadProgress, value); }
        }





        public void Datadownload()
        {
            //数据初始化
            StrartTime = DateTime.Now;
            OriginHtml = new List<string>();
            IsDataDownloading = true;
            Worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,//支持进度信息获取
                WorkerSupportsCancellation = true//支持任务终止
            };

            //信息准备
            links = Book.Sections.Count;
            IPList = HtmlAnalysis.GetIPList(HtmlCrawler.GetHtmlContent(IPNet));


            Worker.DoWork += Work_Download;
            Worker.RunWorkerAsync(links);
            Worker.ProgressChanged += Worker_DownloadProgressChanged;
            Worker.RunWorkerCompleted += Worker_RunDownloadCompleted;
        }

        private void Work_Download(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker backgroundWorker = sender as BackgroundWorker;
            int count = int.Parse(e.Argument.ToString());

            for (int i = 0; i < 10; i++)
            {

                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                GetHtmlContent(IPList[i % IPList.Count], Book.Sections[i].Html);

                backgroundWorker.ReportProgress(i, i.ToString());
            }
            e.Result = null; //事件处理完成之后的结果
        }
        private void Worker_DownloadProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CurrentDownloadProgress = (double)(Convert.ToDecimal(e.ProgressPercentage + 1) / Convert.ToDecimal(links) * 100);
            CurrentDownloadMessage = string.Format("【当前页数:{0} , 剩余{1} , 已消耗时间:{2}】", e.ProgressPercentage + 1, links - e.ProgressPercentage - 1, (DateTime.Now - StrartTime).ToString().Substring(0, 8));
        }
        private void Worker_RunDownloadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("任务已取消");
                CurrentDownloadMessage = "【任务已取消】";
                CurrentDownloadProgress = 0d;
            }
            else if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message + Environment.NewLine + e.Error.Source + Environment.NewLine + e.Error.StackTrace);
            }
            else
            {
                ExtractingData();
                CurrentDownloadMessage = "【任务完成】";
                CurrentDownloadProgress = 100d;
            }
            IsDataDownloading = false;
        }












        private void GetSectionLinks(Novel novel)
        {
            novel.Sections = HtmlAnalysis.AnalysisDirectory(HtmlCrawler.GetHtmlContent(new System.Uri(novel.DirectoryUri)));
        }

        private void GetHtmlContent(string ip, Uri html)
        {
            try
            {
                IPChange.IPChange.SetProxyIP(ip);
                OriginHtml.Add(HtmlCrawler.GetHtmlContent(html));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ExtractingData()
        {
            ParallelLoopResult result = Parallel.ForEach(OriginHtml.ToArray(), (htmlContent, state, i) =>
            {
                book.Sections[(int)i].Content = HtmlAnalysis.AnalysisSectionContent(htmlContent);
            });
        }
    }
}
