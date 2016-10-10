namespace IronRod.Models
{
    public class Volume
    {
        public int id { get; set; }
        public string title { get; set; }
        public string url { get; set; }
    }

    public class Book
    {
        public int id { get; set; } 
        public string title { get; set; } 
        public string url { get; set; } 
        public int volume_id { get; set; } 
    }

    public class Chapter 
    {
        public int id { get; set; }
        public int chapter_number { get; set; }
        public int book_id { get; set; }
    }

    public class Verse
    {
        public int id { get; set; }
        public int verse_number { get; set; }
        public string verse_text { get; set; }
        public int chapter_id { get; set; }
    }
}