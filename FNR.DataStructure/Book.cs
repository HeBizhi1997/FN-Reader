using Nest;

namespace FNR.Model
{
    [ElasticsearchType(IdProperty = "bookid", Name = "book")]
    public class Book
    {
        [Text(Name = "bookid", Index = true)]
        public int BookID { get; set; }

        [Text(Name = "sectionindex", Index = true)]
        public int SectionIndex { get; set; } = 0;
    }
}
