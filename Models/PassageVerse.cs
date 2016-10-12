using System;
using System.Collections.Generic;

namespace IronRod.Models
{
    public class PassageVerse
    {
        public PassageVerse(){}
        public PassageVerse(Passage passage, Verse verse){
            VerseID=verse.id; 
            Passage=passage;
            VerseNumber=verse.verse_number;
            VerseText=verse.verse_text;
            ChapterID=verse.chapter_id;
        }

        public int ID { get; set; }
        public int VerseID { get; set; }  
        public int VerseNumber {get; set;}  
        public string VerseText {get; set;}
        
        public int PassageID { get; set; }
        public Passage Passage {get; set;}
        public int ChapterID {get; set;}
    }
}