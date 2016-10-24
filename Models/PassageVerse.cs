using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 


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
        [Required]
        public int VerseID { get; set; }  
        [Required]
        public int VerseNumber {get; set;}  
        [Required]
        public string VerseText {get; set;}
        public int PassageID { get; set; }
        [Required]
        public Passage Passage {get; set;}
        [Required]
        public int ChapterID {get; set;}
    }
}