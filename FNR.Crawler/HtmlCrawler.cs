using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace FNR.Crawler
{
    public class HtmlCrawler
    {

        /// <summary>
        /// Get the content of a Web page
        /// </summary>
        /// <param name="url">website url</param>
        /// <returns>web content</returns>
        public static string GetHtmlContent(Uri url)
        {
            return GetHtmlContent(url.ToString());
        }

        /// <summary>
        /// Get the Image from a Image Url
        /// </summary>
        /// <param name="url">website url</param>
        /// <returns>web Image</returns>
        public static BitmapFrame GetHtmlImage(Uri url)
        {            
            return Image2Bitmap(GetHtmlImage(url.ToString()));
        }




        #region Private Methods

        /// <summary>
        /// converter System.Drawing.Image to System.Windows.Controls.ImageSource
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private static BitmapFrame Image2Bitmap(Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                var decoder = BitmapDecoder.Create(ms, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                return decoder.Frames[0];
            }
        }

        /// <summary>
        /// Get the encoding method used by the website
        /// </summary>
        /// <param name="url">website url</param>
        /// <returns>web encoding</returns>
        private static Encoding GetEncoding(string url)
        {
            //generate http webRequest
            if (url != null && url != "")
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //use GET method to get url's html
                request.Method = "GET";
                request.Accept = "*/*";
                request.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
                request.ContentType = "text/xml";
                //use webRequest to get response
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Encoding encoding;
                try
                {
                    if (response.CharacterSet != "ISO-8859-1")
                        encoding = Encoding.GetEncoding(response.CharacterSet);
                    else
                        encoding = Encoding.UTF8;
                }
                catch
                {
                    // *** Invalid encoding passed
                    encoding = Encoding.UTF8;
                }
                string sHTML = string.Empty;
                using (StreamReader read = new StreamReader(response.GetResponseStream(), encoding))
                {
                    sHTML = read.ReadToEnd();
                    Match charSetMatch = Regex.Match(sHTML, "charset=(?<code>[a-zA-Z0-9\\-]+)", RegexOptions.IgnoreCase);
                    string sChartSet = charSetMatch.Groups["code"].Value;
                    //if it's not utf-8,we should redecode the html.
                    if (!string.IsNullOrEmpty(sChartSet) && !sChartSet.Equals("utf8", StringComparison.OrdinalIgnoreCase))
                    {
                        encoding = Encoding.GetEncoding(sChartSet);
                    }
                }
                return encoding;
            }
            return Encoding.Default;
        }

        /// <summary>
        /// Get the content of a Web page
        /// </summary>
        /// <param name="url">website url</param>
        /// <returns>web content</returns>
        private static string GetHtmlContent(string url)
        {
            string htmlContent;
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Timeout = 30000;
            webRequest.Method = "GET";
            webRequest.UserAgent = "Mozilla/4.0";
            webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
            if (webResponse.ContentEncoding.ToLower() == "gzip")//if GZip,Unzip

            {
                using (Stream streamReceive = webResponse.GetResponseStream())
                {
                    using (var zipStream =
                        new System.IO.Compression.GZipStream(streamReceive, System.IO.Compression.CompressionMode.Decompress))
                    {
                        //Encoding encoding = GetEncoding(url);
                        using (StreamReader sr = new StreamReader(zipStream, Encoding.UTF8))
                        {
                            htmlContent = sr.ReadToEnd();
                        }
                    }
                }
            }
            else
            {
                using (Stream streamReceive = webResponse.GetResponseStream())
                {
                    //Encoding encoding = GetEncoding(url);
                    using (StreamReader sr = new StreamReader(streamReceive, Encoding.UTF8))
                    {
                        htmlContent = sr.ReadToEnd();
                    }
                }
            }
            return htmlContent;
        }

        /// <summary>
        /// Get the Image from a Image Url
        /// </summary>
        /// <param name="url">website url</param>
        /// <returns>web Image</returns>
        private static Image GetHtmlImage(string url)
        {
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(url);
            Request.Headers.Add("Accept-Encoding", "gzip, deflate");
            HttpWebResponse htmlResponse = (HttpWebResponse)Request.GetResponse();
            using (Stream streamReceive = htmlResponse.GetResponseStream())
            {
                using (var zipStream =
                    new System.IO.Compression.GZipStream(streamReceive, System.IO.Compression.CompressionMode.Decompress))
                {
                    //Encoding enc = GetEncoding(url);
                    using (StreamReader sr = new StreamReader(zipStream, Encoding.UTF8))
                    {
                        return Image.FromStream(sr.BaseStream, true);
                    }
                }
            }
        }

        #endregion
    }
}
