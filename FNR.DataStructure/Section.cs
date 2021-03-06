﻿using Nest;
using System;

namespace FNR.Model
{
    [ElasticsearchType(IdProperty = "bookid", Name = "section")]
    public class Section
    {
        [Text(Name = "bookid", Index = true)]
        public int BookId { get; set; } = -1;//Novel Id
        [Text(Name = "sectionid", Index = true)]
        public int SectionId { get; set; } = -1;//Section Id
        [Text(Name = "name", Index = true, Analyzer = "ik_smart")]
        public string Name { get; set; }    //Section Name
        [Text(Name = "html", Index = true)]
        public Uri Html { get; set; }//Section Uri
        [Text(Name = "content", Index = true, Analyzer = "ik_smart")]
        public string Content { get; set; }//Section Text
    }
}
