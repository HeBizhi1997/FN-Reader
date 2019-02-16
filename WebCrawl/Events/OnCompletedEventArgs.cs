using System;

namespace WebCrawl.Events
{/// <summary>
/// WebCrawl完成事件
/// </summary>
    public class OnCompletedEventArgs
    {
        public Uri Uri { get; private set; } //定义URL地址
        public int ThreadId { get; private set; } //任务线程ID
        public string PageSource { get; private set; } //页面源码
        public long Milliseconds { get; private set; } //WebCrawl请求执行事件
        public OnCompletedEventArgs(Uri uri, int threadId, string pageSource, long milliseconds)
        {
            Uri = uri;
            ThreadId = threadId;
            PageSource = pageSource;
            Milliseconds = milliseconds;
        }
    }
}