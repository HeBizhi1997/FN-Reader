using System;

namespace WebCrawl.Events
{
    /// <summary>
    /// WebCrawl开始事件
    /// </summary>
    public class OnStartEventArgs
    {
        public Uri Uri { get; set; } //定义URL地址
        public OnStartEventArgs(Uri uri)
        {
            Uri = uri;
        }
    }
}