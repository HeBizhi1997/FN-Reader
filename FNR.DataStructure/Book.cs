using Nest;

namespace FNR.Model
{
    [ElasticsearchType(IdProperty = "bookid", Name = "book")]
    public class Book
    {
        [Text(Name = "bookid", Index = false)]
        public int BookID { get; set; }

        [Text(Name = "sectionindex", Index = false)]
        public int SectionIndex { get; set; }
    }
}
