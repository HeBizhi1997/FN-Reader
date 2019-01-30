using System;
using System.Windows.Media.Imaging;

namespace FN_Reader.Models
{
    public class Book
    {
        public string BookName { get; set; }

        public string BookAuthor { set; get; }

        public string LastUpDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");

        public string Lately { get; set; }

        public string Intro { get; set; }

        public BitmapImage Picture { get; set; }

        public string Type { get; set; }
    }
}
