using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FNR.DataStructure;

namespace FNR.Crawler
{
    public class HtmlAnalysis
    {

        /// <summary>
        /// 正则提取代理IP列表 
        /// </summary>
        /// <param name="IpNet"></param>
        /// <returns></returns>
        public static List<string> GetIPList(string htmlContent)
        {
            var _list = new List<string>();
            var table = Regex.Matches(htmlContent, "<table.*?>([\\s\\S]*?)</table>");
            var _tdList = Regex.Matches(table[table.Count - 1].Groups[1].Value, "<td>(.+)</td>");
            for (int i = 1; i < _tdList.Count / 6; i++)
            {
                _list.Add(_tdList[6 * i].Value.Replace("<td>", "").Replace("</td>", "").Trim());
            }

            return _list;
        }


        /// <summary>
        /// 正则提取总榜的所有小说信息
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        public static List<Novel> AnalysisTotalRank(string htmlContent)
        {
            var _List = new List<Novel>();

            MatchCollection L = Regex.Matches(htmlContent, @"<td class=""L""><a href=""(https://www.23us.so/xiaoshuo/([0-9]+).html)"">(.+)</a></td>?");
            MatchCollection C = Regex.Matches(htmlContent, @"<td class=""C"">(.+)</td>");

            for (int index = 0; index < L.Count; index++)
            {
                _List.Add(new Novel
                {
                    Name = L[index].Groups[3].Value.Trim(),
                    Author = C[3 * index].Groups[1].Value.Trim(),
                    Update = DateTime.Parse(C[3 * index + 1].Groups[1].Value.Trim()),
                    State = C[3 * index + 2].Groups[1].Value.Trim(),
                    Uri = L[index].Groups[1].Value.Trim(),
                    Id = int.Parse(L[index].Groups[2].Value.Trim())
                });
            }

            return _List;
        }

        /// <summary>
        /// 正则提取首页的信息
        /// </summary>
        /// <param name="novel"></param>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        public static Novel AnalysisIndexPage(Novel novel, string htmlContent)
        {
            novel.Intro = Regex.Match(htmlContent, "<p>([^<]+?)<br[^/]+/>").Groups[1].Value.Replace("&nbsp;", "").Trim();
            //novel.Cover = new Crawler.Crawler().GetHtmlImage(Regex.Match(htmlContent, "https://www.23us.so/files/article/image/.+s.jpg").Value);
            novel.Cover = Regex.Match(htmlContent, "https://www.23us.so/files/article/image/.+s.jpg").Value.Trim();
            novel.Lately = Regex.Match(Regex.Match(htmlContent, "最近章节[\\s\\S]+?</a>").Value, ">(.+)<").Groups[1].Value.Trim();
            novel.DirectoryUri = Regex.Match(htmlContent, "https://www.23us.so/.+/index.html").Value.Trim();
            novel.Type = Regex.Match(Regex.Match(htmlContent, "小说类别[\\s\\S]+?</a>").Value, "<a[^>]+>(.+)</a>").Groups[1].Value.Trim();

            return novel;
        }

        /// <summary>
        /// 正则提取目录
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        public static List<Section> AnalysisDirectory(string htmlContent)
        {
            var _List = new List<Section>();

            //获取章节目录
            var mat_mulu = new Regex(@"<table cellspacing=""1"" cellpadding=""0"" bgcolor=""#E4E4E4"" id=""at"">(.|\n)*?</table>").Match(htmlContent);
            // 匹配a标签里面的url
            MatchCollection sMC = new Regex("<a[^>]+?href=\"([^\"]+)\"[^>]*>([^<]+)</a>", RegexOptions.Compiled).Matches(mat_mulu.Groups[0].ToString());

            for (int index = 0; index < sMC.Count; index++)
            {
                var html = Regex.Match(sMC[index].ToString(), @"""(.+)""").Groups[1].Value;
                _List.Add(new Section()
                {
                    Name = Regex.Match(sMC[index].ToString(), @">(.+)<").Groups[1].Value,
                    Html = new Uri(html),
                    SectionId = index,
                    BookId = int.Parse(html.Split('/')[html.Split('/').Length - 2])
                });
            }

            return _List;
        }

        /// <summary>
        /// 正则提取每个章节的内容
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        public static string AnalysisSectionContent(string htmlContent)
        {
            return Regex.Replace(
                new Regex(@"<dd id=""contents"">(.|\n)*?</dd>")
                .Match(htmlContent).Groups[0].ToString()
                .Replace("<dd id=\"contents\">", "")
                .Replace("</dd>", "")
                .Replace("&nbsp;", "")
                .Replace("<br />", "\r\n")
                , @"\s\s\s\s\s\s\s\s\s\s", "\r\n");
        }
    }
}
