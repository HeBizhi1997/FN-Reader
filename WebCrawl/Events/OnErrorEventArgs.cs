using System;

namespace WebCrawl.Events
{
    /// <summary>
    /// WebCrawl出错事件
    /// </summary>
    public class OnErrorEventArgs
    {

        public Uri Uri { get; set; }
        public Exception Exception { get; set; }

        public OnErrorEventArgs(Uri uri, Exception exception)
        {
            Uri = uri;
            Exception = exception;
        }
    }
}
