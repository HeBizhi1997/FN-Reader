using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FNR.Crawler;
using FNR.Model;
using Microsoft.Win32;
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
        private string path;

        public DelegateCommand<object> WinCloseCommand { get; set; }
        public DelegateCommand<object> WinClosingCommand { get; set; }
        public DelegateCommand<object> WinMaximizeCommand { get; set; } = new DelegateCommand<object>((p) => (p as Window).WindowState = (p as Window).WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized);
        public DelegateCommand<object> WinMinimizeCommand { get; set; } = new DelegateCommand<object>((p) => SystemCommands.MinimizeWindow(p as Window));
        public DelegateCommand<object> WinMoveCommand { get; set; } = new DelegateCommand<object>((p) => (p as Window).DragMove());
        public DelegateCommand<object> SelectItemChangedCommand { get; set; }
        public DelegateCommand DownloadSectionsCommand { get; set; }

        public ReadWndViewModel(object data)
        {
            Book = data as Novel;
            Reader = (Application.Current.MainWindow.DataContext as StartWinodwViewModel).Reader;

            GetSectionLinks(Book);

            SelectItemChangedCommand = new DelegateCommand<object>((p) =>
            {
                if (p is ListView listView)
                {
                    var htmlContent = HtmlCrawler.GetHtmlContent(Book.Sections[listView.SelectedIndex].Html);
                    CurrentContent = HtmlAnalysis.AnalysisSectionContent(htmlContent);
                }
            });

            DownloadSectionsCommand = new DelegateCommand(() =>
            {
                SaveFileDialog dialog = new SaveFileDialog
                {
                    Filter = "txt files(*.txt)|*.txt|word files(*.doc)|*.doc|All files(*.*)|*.*",
                    FileName = Book.Name,
                    DefaultExt = "txt"
                };
                if (dialog.ShowDialog() == true)
                {
                    path = dialog.FileName;
                    Datadownload();
                }
            });

            int index = 0;
            if (Reader.Level >= 0)
                foreach (var item in Reader.Books)
                {
                    if (Book.Id == item.BookID)
                    {
                        index = item.SectionIndex;
                        break;
                    }
                }

            CurrentContent = HtmlAnalysis.AnalysisSectionContent(HtmlCrawler.GetHtmlContent(Book.Sections[(index - 1) >= 0 ? (index - 1) : 0].Html));

            SysFontFamilies = Fonts.SystemFontFamilies;

            WinCloseCommand = new DelegateCommand<object>((p) =>
            {
                (p as Window).Close();
                Application.Current.MainWindow.Show();
            });

            WinClosingCommand = new DelegateCommand<object>((p) =>
              {
                  //若非访客模式
                  if (reader.Level >= 0)
                      //若书架上已存在该书 则更新本次阅读进度
                      if (index > 0)
                      {
                          if ((p as ListView).SelectedIndex < 0)
                              (p as ListView).SelectedIndex = index - 1;
                          Reader.Books.Find(b => b.BookID == Book.Id).SectionIndex = (p as ListView).SelectedIndex + 1;
                          ElasticSearch.ElasticHelper.Insert(Reader);
                      }
                      //若书架尚不存在此书 则新添加入列表
                      else
                      {
                          if (MessageBox.Show("是否加入书架?") == MessageBoxResult.OK)
                          {
                              if ((p as ListView).SelectedIndex < 0)
                                  (p as ListView).SelectedIndex = index - 1;

                              Reader.Books.Add(new Model.Book()
                              {
                                  BookID = Book.Id,
                                  SectionIndex = (p as ListView).SelectedIndex + 1
                              });
                              ElasticSearch.ElasticHelper.Insert(Reader);
                          }
                      }
              });
        }

        private User reader;
        private Novel book;
        private string currentContent;
        private bool isDataNotDownloading = true;
        private string currentDownloadMessage = string.Empty;
        private double currentDownloadProgress = 0d;
        private ICollection<FontFamily> sysFontFamilies = new List<FontFamily>();


        public User Reader
        {
            get { return reader; }
            set { SetProperty(ref reader, value); }
        }
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
        public bool IsDataNotDownloading
        {
            get { return isDataNotDownloading; }
            set { SetProperty(ref isDataNotDownloading, value); }
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
        public ICollection<FontFamily> SysFontFamilies
        {
            get { return sysFontFamilies; }
            set { SetProperty(ref sysFontFamilies, value); }
        }


        private void Datadownload()
        {
            //数据初始化
            StrartTime = DateTime.Now;
            OriginHtml = new List<string>();
            IsDataNotDownloading = false;
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

            for (int i = 0; i < count; i++)
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
            IsDataNotDownloading = true;
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
            var Content = string.Empty;
            int index = 0;
            foreach (var htmlContent in OriginHtml)
            {
                Content += book.Sections[index].Name + Environment.NewLine + HtmlAnalysis.AnalysisSectionContent(htmlContent) + Environment.NewLine;
                index++;
            }
            File.WriteAllText(path, Content);
            MessageBox.Show("保存成功！");
        }
    }
}
