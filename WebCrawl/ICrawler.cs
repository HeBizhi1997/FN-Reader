using System;
using System.Threading.Tasks;
using WebCrawl.Events;

namespace WebCrawl
{
    public interface ICrawler
    {
        event EventHandler<OnStartEventArgs> OnStart;//WebCrawl启动事件

        event EventHandler<OnCompletedEventArgs> OnCompleted;//WebCrawl完成事件

        event EventHandler<OnErrorEventArgs> OnError;//WebCrawl出错事件

        Task<string> Start(Uri uri, string proxy); //异步WebCrawl
    }
}
