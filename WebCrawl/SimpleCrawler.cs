using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebCrawl.Events;

namespace WebCrawl
{
    public class SimpleCrawler : ICrawler
    {
        public event EventHandler<OnStartEventArgs> OnStart;     //爬取开始
        public event EventHandler<OnCompletedEventArgs> OnCompleted;  //爬取完成
        public event EventHandler<OnErrorEventArgs> OnError;       //爬取出错

        public CookieContainer CookiesContainer { get; set; } //定义Cookie容器

        public SimpleCrawler() { }


        /// <summary>
        /// 异步创建WebCrawl
        /// </summary>
        /// <param name="uri">WebCrawlURL地址</param>
        /// <param name="proxy">代理服务器</param>
        /// <returns>网页源代码</returns>
        public async Task<string> Start(Uri uri, string proxy = null)
        {
            return await Task.Run(() =>
            {
                var pageSource = string.Empty;
                try
                {

                    OnStart?.Invoke(this, new OnStartEventArgs(uri));

                    var watch = new Stopwatch();
                    watch.Start();

                    var request = (HttpWebRequest)WebRequest.Create(uri);
                    request.Accept = "*/*";
                    request.ServicePoint.Expect100Continue = false;//加快载入速度
                    request.ServicePoint.UseNagleAlgorithm = false;//禁止Nagle算法加快载入速度
                    request.AllowWriteStreamBuffering = false;//禁止缓冲加快载入速度
                    request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");//定义gzip压缩页面支持
                    request.ContentType = "application/x-www-form-urlencoded";//定义文档类型及编码
                    request.AllowAutoRedirect = false;//禁止自动跳转

                    //设置User-Agent，伪装成Google Chrome浏览器
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36";
                    request.Timeout = 5000;//定义请求超时时间为5秒
                    request.KeepAlive = true;//启用长连接
                    request.Method = "GET";//定义请求方式为GET              

                    if (proxy != null)
                        request.Proxy = new WebProxy(proxy);//设置代理服务器IP，伪装请求地址

                    request.CookieContainer = this.CookiesContainer;//附加Cookie容器
                    request.ServicePoint.ConnectionLimit = int.MaxValue;//定义最大连接数

                    using (var response = (HttpWebResponse)request.GetResponse())
                    {
                        //获取请求响应
                        foreach (Cookie cookie in response.Cookies) this.CookiesContainer.Add(cookie);//将Cookie加入容器，保存登录状态
                        if (response.ContentEncoding.ToLower().Contains("gzip"))//解压
                        {
                            using (GZipStream stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                            {
                                using (StreamReader reader = new StreamReader(stream, Encoding.Default))
                                {
                                    pageSource = reader.ReadToEnd();
                                }
                            }
                        }
                        else if (response.ContentEncoding.ToLower().Contains("deflate"))//解压
                        {
                            using (DeflateStream stream = new DeflateStream(response.GetResponseStream(), CompressionMode.Decompress))
                            {
                                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                                {
                                    pageSource = reader.ReadToEnd();
                                }
                            }
                        }
                        else
                        {
                            using (Stream stream = response.GetResponseStream())//原始
                            {
                                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                                {
                                    pageSource = reader.ReadToEnd();
                                }
                            }
                        }
                    }

                    request.Abort();
                    watch.Stop();

                    var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;//获取当前任务线程ID
                    var milliseconds = watch.ElapsedMilliseconds;//获取请求执行时间
                    OnCompleted?.Invoke(this, new OnCompletedEventArgs(uri, threadId, pageSource, milliseconds));
                }
                catch (Exception ex)
                {
                    OnError?.Invoke(this, new OnErrorEventArgs(uri, ex));
                }
                return pageSource;
            });
        }

        public async Task<string> StartContent(Uri uri, string proxy = null)
        {
            return await Task.Run(() =>
            {
                var pageSource = string.Empty;
                try
                {
                    OnStart?.Invoke(this, new OnStartEventArgs(uri));

                    var watch = new Stopwatch();
                    watch.Start();

                    var request = (HttpWebRequest)WebRequest.Create(uri.OriginalString);
                    request.Accept = "*/*";
                    request.ServicePoint.Expect100Continue = false;//加快载入速度
                    request.ServicePoint.UseNagleAlgorithm = false;//禁止Nagle算法加快载入速度
                    request.AllowWriteStreamBuffering = false;//禁止缓冲加快载入速度
                    request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");//定义gzip压缩页面支持
                    request.ContentType = "application/x-www-form-urlencoded";//定义文档类型及编码
                    request.AllowAutoRedirect = false;//禁止自动跳转

                    //设置User-Agent，伪装成Google Chrome浏览器
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36";
                    request.Timeout = 5000;//定义请求超时时间为5秒
                    request.KeepAlive = true;//启用长连接
                    request.Method = "GET";//定义请求方式为GET              
                    request.Referer = uri.ToString();

                    if (proxy != null)
                        request.Proxy = new WebProxy(proxy);//设置代理服务器IP，伪装请求地址

                    request.CookieContainer = this.CookiesContainer;//附加Cookie容器
                    request.ServicePoint.ConnectionLimit = int.MaxValue;//定义最大连接数

                    using (var response = (HttpWebResponse)request.GetResponse())
                    {
                        //获取请求响应
                        foreach (Cookie cookie in response.Cookies) this.CookiesContainer.Add(cookie);//将Cookie加入容器，保存登录状态
                        if (response.ContentEncoding.ToLower().Contains("gzip"))//解压
                        {
                            using (GZipStream stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                            {
                                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                                {
                                    pageSource = reader.ReadToEnd();
                                }
                            }
                        }
                        else if (response.ContentEncoding.ToLower().Contains("deflate"))//解压
                        {
                            using (DeflateStream stream = new DeflateStream(response.GetResponseStream(), CompressionMode.Decompress))
                            {
                                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                                {
                                    pageSource = reader.ReadToEnd();
                                }
                            }
                        }
                        else
                        {
                            using (Stream stream = response.GetResponseStream())//原始
                            {
                                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                                {
                                    pageSource = reader.ReadToEnd();
                                }
                            }
                        }
                    }

                    request.Abort();
                    watch.Stop();

                    var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;//获取当前任务线程ID
                    var milliseconds = watch.ElapsedMilliseconds;//获取请求执行时间
                    OnCompleted?.Invoke(this, new OnCompletedEventArgs(uri, threadId, pageSource, milliseconds));
                }
                catch (Exception ex)
                {
                    OnError?.Invoke(this, new OnErrorEventArgs(uri, ex));
                }
                return pageSource;
            });
        }

    }
}
