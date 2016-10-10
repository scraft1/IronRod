using System;
using System.Collections.Generic;

namespace IronRod.Models
{
    public class PassageVerse
    {
        public int ID { get; set; }
        public int VerseID { get; set; }  
        public int VerseNumber {get; set;}  
        public string VerseText {get; set;}
        
        public int PassageID { get; set; }
        public Passage Passage {get; set;}
    }
}