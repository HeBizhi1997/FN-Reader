using System;

namespace FNR.DataStructure
{
    public class Section
    {
        public int BookId { get; set; } = -1;//Novel Id
        public int SectionId { get; set; } = -1;//Section Id
        public string Name { get; set; }    //Section Name
        public Uri Html { get; set; }//Section Uri
        public string Content { get; set; }//Section Text
    }
}
