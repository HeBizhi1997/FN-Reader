using System;
using System.Collections.Generic;
using System.ComponentModel;
using FNR.Model;
using Prism.Commands;
using Prism.Mvvm;
using FNR.Crawler;
using System.Windows;
using System.Threading.Tasks;
using FNR.ElasticSearch;

namespace FNR.ViewModel
{
    public class DataDownloadViewModel : BindableBase
    {
        private readonly Uri UriRoot = new Uri("https://www.23us.so/top/allvisit_1.html");//总榜首页
        private readonly string UriPart1 = "https://www.23us.so/top/allvisit_";
        private readonly string UriPart2 = ".html";

        private readonly Uri IPNet = new Uri("http://www.shenjidaili.com/open/");//IP代理网站
        private List<string> IPList = new List<string>();//存放获取的代理IP
        private List<string> OriginHtml = new List<string>();//存放原始网页
        private DateTime StrartTime = DateTime.Now; //任务开始时间
        private BackgroundWorker Worker;
        private int links = 0;






        public DataDownloadViewModel()
        {
            DownloadTotalRankCommand = new DelegateCommand(() => TotalRankDatadownload());
            CancelCommand = new DelegateCommand(() => Worker.CancelAsync());
            DownloadHomePageCommand = new DelegateCommand(() => HomePageDatadownload());
        }

        private List<Novel> novelList = new List<Novel>();
        private double currentDownloadTotalRankProgress = 0d;
        private string currentDownloadTotalRankMessage;
        private bool isWorkNotRuning = true;
        private double currentDownloadHomePageProgress = 0d;
        private string currentDownloadHomePageMessage;


        public DelegateCommand DownloadTotalRankCommand { get; set; }
        public DelegateCommand DownloadHomePageCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }


        public List<Novel> NovelList
        {
            get { return novelList; }
            set { SetProperty(ref novelList, value); }
        }
        public double CurrentDownloadTotalRankProgress
        {
            get { return currentDownloadTotalRankProgress; }
            set { SetProperty(ref currentDownloadTotalRankProgress, value); }
        }
        public string CurrentDownloadTotalRankMessage
        {
            get { return currentDownloadTotalRankMessage; }
            set { SetProperty(ref currentDownloadTotalRankMessage, value); }

        }

        public bool IsWorkNotRuning
        {
            get { return isWorkNotRuning; }
            set { SetProperty(ref isWorkNotRuning, value); }

        }

        public double CurrentDownloadHomePageProgress
        {
            get { return currentDownloadHomePageProgress; }
            set { SetProperty(ref currentDownloadHomePageProgress, value); }
        }
        public string CurrentDownloadHomePageMessage
        {
            get { return currentDownloadHomePageMessage; }
            set { SetProperty(ref currentDownloadHomePageMessage, value); }

        }








        private void TotalRankDatadownload()
        {
            //数据初始化
            StrartTime = DateTime.Now;
            OriginHtml = new List<string>();
            IsWorkNotRuning = false;
            NovelList = new List<Novel>();
            Worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,//支持进度信息获取
                WorkerSupportsCancellation = true//支持任务终止
            };

            //信息准备
            links = HtmlAnalysis.AnalysisTotalRankPageCount(HtmlCrawler.GetHtmlContent(UriRoot));
            IPList = HtmlAnalysis.GetIPList(HtmlCrawler.GetHtmlContent(IPNet));


            Worker.DoWork += Work_DownloadTotalRank;
            Worker.RunWorkerAsync(links);
            Worker.ProgressChanged += Worker_DownloadTotalRankProgressChanged;
            Worker.RunWorkerCompleted += Worker_RunDownloadTotalRankCompleted;
        }

        private void Worker_RunDownloadTotalRankCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("任务已取消");
                CurrentDownloadTotalRankMessage = "【任务已取消】";
                CurrentDownloadTotalRankProgress = 0d;
            }
            else if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message + Environment.NewLine + e.Error.Source + Environment.NewLine + e.Error.StackTrace);
            }
            else
            {
                ExtractingTotalRankData();
                CurrentDownloadTotalRankMessage = "【任务完成】";
                CurrentDownloadTotalRankProgress = 100d;
            }
            IsWorkNotRuning = true;
        }

        private void Worker_DownloadTotalRankProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CurrentDownloadTotalRankProgress = (double)(Convert.ToDecimal(e.ProgressPercentage + 1) / Convert.ToDecimal(links) * 100);
            CurrentDownloadTotalRankMessage = string.Format("【当前页数:{0} , 剩余{1} , 已消耗时间:{2}】", e.ProgressPercentage + 1, links - e.ProgressPercentage - 1, (DateTime.Now - StrartTime).ToString().Substring(0, 8));
        }

        private void Work_DownloadTotalRank(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker backgroundWorker = sender as BackgroundWorker;
            int count = int.Parse(e.Argument.ToString());

            for (int i = 0; i < count; i++)
            {

                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                GetHtmlContent(IPList[i % IPList.Count], UriPart1 + (i + 1) + UriPart2);

                backgroundWorker.ReportProgress(i, i.ToString());
            }
            e.Result = null; //事件处理完成之后的结果
        }



        private void HomePageDatadownload()
        {
            //数据初始化
            StrartTime = DateTime.Now;
            OriginHtml = new List<string>();
            IsWorkNotRuning = false;
            Worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,//支持进度信息获取
                WorkerSupportsCancellation = true//支持任务终止
            };

            //信息准备
            links = NovelList.Count;
            IPList = HtmlAnalysis.GetIPList(HtmlCrawler.GetHtmlContent(IPNet));


            Worker.DoWork += Work_DownloadHomePage;
            Worker.RunWorkerAsync(links);
            Worker.ProgressChanged += Worker_DownloadHomePageProgressChanged;
            Worker.RunWorkerCompleted += Worker_RunDownloadHomePageCompleted;
        }

        private void Worker_RunDownloadHomePageCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("任务已取消");
                CurrentDownloadHomePageMessage = "【任务已取消】";
                CurrentDownloadHomePageProgress = 0d;
            }
            else if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message + Environment.NewLine + e.Error.Source + Environment.NewLine + e.Error.StackTrace);
            }
            else
            {
                ExtractingHomePageData();

                InsertOrUpdateDate();

                CurrentDownloadHomePageMessage = "【任务完成】";
                CurrentDownloadHomePageProgress = 100d;
            }
            IsWorkNotRuning = true;

        }

        private void Worker_DownloadHomePageProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CurrentDownloadHomePageProgress = (double)(Convert.ToDecimal(e.ProgressPercentage + 1) / Convert.ToDecimal(links) * 100);
            CurrentDownloadHomePageMessage = string.Format("【当前页数:{0} , 剩余{1} , 已消耗时间:{2}】", e.ProgressPercentage + 1, links - e.ProgressPercentage - 1, (DateTime.Now - StrartTime).ToString().Substring(0, 8));
        }

        private void Work_DownloadHomePage(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker backgroundWorker = sender as BackgroundWorker;
            int count = int.Parse(e.Argument.ToString());

            for (int i = 0; i < count; i++)
            {

                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                GetHtmlContent(IPList[i % IPList.Count], NovelList[i].Uri);

                backgroundWorker.ReportProgress(i, i.ToString());
            }
            e.Result = null; //事件处理完成之后的结果
        }



        private void InsertOrUpdateDate()
        {
            ElasticHelper.CreateIndex();
            foreach (var item in NovelList)
            {
                ElasticHelper.Insert(item);
            }
        }












        /// <summary>
        /// 跟换IP 获取网页
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="html"></param>
        private void GetHtmlContent(string ip, string html)
        {
            try
            {
                IPChange.IPChange.SetProxyIP(ip);
                OriginHtml.Add(HtmlCrawler.GetHtmlContent(new Uri(html)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 提取总榜网页信息
        /// </summary>
        private void ExtractingTotalRankData()
        {
            ParallelLoopResult result = Parallel.ForEach(OriginHtml.ToArray(), (htmlContent, state, i) =>
            {
                foreach (var item in HtmlAnalysis.AnalysisTotalRank(htmlContent))
                {
                    NovelList.Add(item);
                }
            });
        }

        /// <summary>
        /// 提取各本书的首页信息
        /// </summary>
        private void ExtractingHomePageData()
        {
            ParallelLoopResult result = Parallel.ForEach(OriginHtml.ToArray(), (htmlContent, state, i) =>
            {
                NovelList[(int)i] = HtmlAnalysis.AnalysisHomePage(NovelList[(int)i], htmlContent);
            });
        }
    }
}