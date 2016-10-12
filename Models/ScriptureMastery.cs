namespace IronRod.Models
{
    public class SMVolume
    {
        public int id { get; set; }
        public string title { get; set; }
    }
    public class Set
    {
        public int id { get; set; }
        public string title { get; set; }
        public int volume_id { get; set; }
    }
    public class SMVerse
    {
        public int ID { get; set; }
        public int verse_id { get; set; }
        public int set_id { get; set; }
    }
   
}
