
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using WebCrawl.Models;
using System.Drawing;
using System.Threading;

namespace WebCrawl
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            NovelCrawler("..\\剑来\\", "75_75584");

            Console.ReadKey();
        }


        public static void NovelCrawler(string path, string NId)
        {
            var novel = new Novel();
            var NovelNet = "https://www.biquge5200.cc/" + NId + "/";
            var novelCrawler = new SimpleCrawler();

            novelCrawler.OnStart += (s, e) =>
            {
                Console.WriteLine("爬虫开始抓取地址：" + e.Uri.ToString());
            };

            novelCrawler.OnError += (s, e) =>
            {
                Console.WriteLine("爬虫抓取出现错误：" + e.Uri.ToString() + "，异常消息：" + e.Exception.Message);
            };

            novelCrawler.OnCompleted += (s, e) =>
            {
                var type = Regex.Matches(e.PageSource, "<a href=\"/\">笔趣阁</a> &gt; <a href=\"[^\"]*\">([^<]*)</a>", RegexOptions.IgnoreCase);
                foreach (Match v in type)
                    novel.Type = v.Groups[1].ToString();

                var image = Regex.Matches(e.PageSource, "<div id=\"fmimg\"><img alt=\"\" src=\"([^\"]*)\"[^>]*>", RegexOptions.IgnoreCase);
                foreach (Match v in image)
                {
                    using (var web = new WebClient())
                    {
                        web.DownloadFile(v.Groups[1].ToString(), path + "封面.jpg");
                    }
                    novel.Picture = new Bitmap(path + "封面.jpg");
                }

                var info = Regex.Matches(e.PageSource, "<div id=\"info\">[^<]*<h1>([^<]+)</h1>[^<]*<p[^>]*>作&nbsp;&nbsp;&nbsp;&nbsp;者：([^<]+)</p>[^<]*<p[^>]*>(?!</p>).*</p>[^<]*<p>最后更新：([^<]*)</p>[^<]*</div>[^<]*<div id=\"intro\">[^<]*<p><p>([^<]*)</p></p>[^<]*</div>", RegexOptions.IgnoreCase);
                foreach (Match match in info)
                {
                    novel.Name = match.Groups[1].Value.ToString();
                    novel.Author = match.Groups[2].Value.ToString();
                    novel.Update = DateTime.Parse(match.Groups[3].ToString());
                    novel.Intro = match.Groups[4].ToString().Replace("\t", "").Replace("\r\n", "").Trim(); ;
                }

                var links = Regex.Matches(e.PageSource, "<a href=\"" + NovelNet + "([0-9]+).html\">([^<]*)</a>", RegexOptions.IgnoreCase);
                foreach (Match match in links)
                {
                    novel.Sections.Add(match.Groups[2].Value.ToString());
                    novel.Id = int.Parse(match.Groups[1].Value.ToString());
                    novel.SectionUri.Add(new Uri(e.Uri + match.Groups[1].Value.ToString() + ".html"));
                }

                Console.WriteLine();
                Console.WriteLine("===============================================");
                Console.WriteLine("爬虫抓取任务完成！合计 " + links.Count + " 个章节。");
                Console.WriteLine("地址：" + e.Uri.ToString());
            };
            novelCrawler.Start(new Uri(NovelNet)).Wait();


            //逐章抓取生成一个txt 超级慢
            foreach (var v in novel.SectionUri)
            {
                NovelCrawlerContent(novel, v, path);
                Thread.Sleep(350);
            }
            //并发抓取 生成多个txt
            //NovelCrawlerContent(novel, path);
        }


        /// <summary>
        /// 章节抓取
        /// </summary>
        public static void NovelCrawlerContent(Novel novel, Uri uri, string path)
        {
            var novelCrawler = new SimpleCrawler();

            novelCrawler.OnStart += (s, e) =>
            {
                Console.WriteLine("爬虫开始抓取地址：" + e.Uri.ToString());
            };

            novelCrawler.OnError += (s, e) =>
            {
                Console.WriteLine("爬虫抓取出现错误：" + e.Uri.ToString() + "，异常消息：" + e.Exception.Message);
            };

            novelCrawler.OnCompleted += (s, e) =>
            {
                var name = Regex.Match(e.PageSource, "<div class=\"bookname\">[^<]*<h1>([^<]*)</h1>");
                var content = Regex.Match(e.PageSource, @"<div id=""content"">(.|\n)*?</div>", RegexOptions.IgnoreCase);

                novel.SectionContent.Add(name.Groups[1].Value.ToString() + "\r\n" + content.Groups[0].Value.ToString().Replace("</p>", "\r\n").Replace("<script>", "").Replace("<div id=\"content\">", "").Replace("<p>", "").Replace("</div>", ""));

                Console.WriteLine(name.Groups[1].Value.ToString().Trim());

                using (FileStream fs = new FileStream(path + name.Groups[1].Value.ToString().Trim().Replace(":", "：").Replace("?", "？").Replace("*", "_").Replace("\"", "“") + ".txt", FileMode.OpenOrCreate))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        sw.Write(novel.SectionContent.Last().ToString());
                    }
                }
            };
            novelCrawler.Start(uri).Wait();
        }


        /// <summary>
        /// 并发抓取全本小说
        /// </summary>
        public static void NovelCrawlerContent(Novel novel, string path)
        {
            var novelCrawler = new SimpleCrawler();

            novelCrawler.OnStart += (s, e) =>
            {
                Console.WriteLine("爬虫开始抓取地址：" + e.Uri.ToString());
            };

            novelCrawler.OnError += (s, e) =>
            {
                Console.WriteLine("爬虫抓取出现错误：" + e.Uri.ToString() + "，异常消息：" + e.Exception.Message);
            };

            novelCrawler.OnCompleted += (s, e) =>
            {
                var name = Regex.Match(e.PageSource, "<div class=\"bookname\">[^<]*<h1>([^<]*)</h1>");
                var content = Regex.Match(e.PageSource, @"<div id=""content"">(.|\n)*?</div>", RegexOptions.IgnoreCase);

                novel.SectionContent.Add(name.Groups[1].Value.ToString() + "\r\n" + content.Groups[0].Value.ToString().Replace("</p>", "\r\n").Replace("<script>", "").Replace("<div id=\"content\">", "").Replace("<p>", "").Replace("</div>", ""));

                Console.WriteLine(name.Groups[1].Value.ToString().Trim());

                using (FileStream fs = new FileStream(path + name.Groups[1].Value.ToString().Trim().Replace(":", "：").Replace("?", "？").Replace("*", "_").Replace("\"", "“") + ".txt", FileMode.OpenOrCreate))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        sw.Write(novel.SectionContent.Last().ToString());
                    }
                }

                Console.WriteLine();
                Console.WriteLine("===============================================");
                Console.WriteLine("爬虫抓取任务完成！");
                Console.WriteLine("耗时：" + e.Milliseconds + "毫秒");
                Console.WriteLine("线程：" + e.ThreadId);
                Console.WriteLine("地址：" + e.Uri.ToString());
            };

            Parallel.For(0, novel.Sections.Count, async (i) =>
            {
                Thread.Sleep(5000);
                var v = novel.SectionUri[i];
                await novelCrawler.Start(new Uri(v.ToString()));
            });
        }

    }

}
