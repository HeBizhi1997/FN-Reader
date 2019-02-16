using System;
using System.Collections.Generic;
using System.Drawing;


namespace WebCrawl.Models
{
    public class Novel
    {
        public int Id { get; set; }    //书编号
        public string Name { get; set; }    //书名
        public string Author { get; set; } //作者
        public DateTime Update { get; set; } //最近更新日期
        public string Lately { get; set; }  //最新章节
        public string Intro { get; set; } //简介
        public string Type { get; set; }    //类型
        public Image Picture { get; set; } //封面



        public List<string> Sections { get; set; } = new List<string>();//章节列表
        public List<Uri> SectionUri { get; set; } = new List<Uri>(); //章节链接列表
        public List<string> SectionContent { get; set; } = new List<string>();//章节内容列表
    }
}
