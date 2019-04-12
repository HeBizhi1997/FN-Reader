using System;
using System.Text;
using System.Drawing;


namespace FNR.Crawler
{
    public interface ICrawler
    {
        /// <summary>
        /// Get the content of a Web page
        /// </summary>
        /// <param name="url">website url</param>
        /// <returns>web content</returns>
        string GetHtmlContent(Uri url);

        /// <summary>
        /// Get the Image from a Image Url
        /// </summary>
        /// <param name="url">website url</param>
        /// <returns>web Image</returns>
        Image GetHtmlImage(Uri url);
    }
}
