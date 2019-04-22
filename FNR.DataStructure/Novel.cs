using Nest;
using System;
using System.Collections.Generic;

namespace FNR.Model
{
    [ElasticsearchType(IdProperty = "id", Name = "novel")]
    public class Novel
    {
        [Text(Name = "id", Index = false)]
        public int Id { get; set; } = -1;
        [Text(Name = "name", Index = true, Analyzer = "ik_smart")]
        public string Name { get; set; }
        [Text(Name = "author", Index = true, Analyzer = "ik_smart")]
        public string Author { get; set; }
        [Text(Name = "update", Index = false)]
        public DateTime Update { get; set; } //Recent Updates
        [Text(Name = "lately", Index = false)]
        public string Lately { get; set; }  //Lately section
        [Text(Name = "intro", Index = true,Analyzer ="ik_smart")]
        public string Intro { get; set; }
        [Text(Name = "type", Index = true)]
        public string Type { get; set; }
        [Text(Name = "cover", Index = false)]
        public string Cover { get; set; } //Novel Cover uri
        [Text(Name = "uri", Index = false)]
        public string Uri { get; set; }//Homepage uri
        [Text(Name = "state", Index = false)]
        public string State { get; set; }//State: updating or finished
        [Text(Name = "directoryUri", Index = false)]
        public string DirectoryUri { get; set; }//Novel directory uri
        [Text(Name = "section", Index = false)]
        public List<Section> Sections { get; set; } = new List<Section>();//Section List
    }
}
