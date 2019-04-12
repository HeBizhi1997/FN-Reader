using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNR.Model
{
    public class NovelModel
    {
        public int Id { get; set; } = -1;  
        public string Name { get; set; }  
        public string Author { get; set; } 
        public DateTime Update { get; set; } //Recent Updates
        public string Lately { get; set; }  //Lately section
        public string Intro { get; set; } 
        public string Type { get; set; } 
        public string Cover { get; set; } //Novel Cover uri
        public string Uri { get; set; }//Homepage uri
        public string State { get; set; }//State: updating or finished
        public string DirectoryUri { get; set; }//Novel directory uri

        public List<Section> Sections { get; set; } = new List<Section>();//Section List
    }
}
