using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 

namespace IronRod.Models
{
    public class Passage
    {
        public int ID { get; set; }
        [Required]
        public string UserName {get; set;}
        [Required]
        public string Title {get; set;}
        public int Level {get; set;} = 0; 
        public DateTime DatePassed {get; set;} = DateTime.Today;
        public DateTime DateCreated {get;set;} = DateTime.Now;
        public int FirstVerse {get;set;} // used for sorting

        public ICollection<PassageVerse> Verses { get; set; }
        public ICollection<PassageTopic> PassageTopics { get; set; }

        public void Passed() {
            if(DatePassed < DateTime.Today || Level == 0){
                Level = Level + 1;
                DatePassed = DateTime.Today;
            }
        }
    }

    // VIEW MODELS 

    public class PassageViewModel
    {
        public int ID {get; set;}
        public string Title {get; set;}
        public int Level {get; set;}
        public DateTime DatePassed {get; set;}
        public DateTime DateCreated {get; set;}
        public int FirstVerse {get; set;}
    }

    public class PassageDetailViewModel
    {
        public int ID {get; set;}
        public string Title {get; set;}
        public int Level {get; set;}
        public DateTime DatePassed {get; set;}
        public IEnumerable<PassageVerseViewModel> Verses {get; set;}
    }

    public class PassageBackup {
        public string Title {get; set;}
        public int Level {get; set;}
        public DateTime DatePassed {get; set;}
        public DateTime DateCreated {get; set;}
        public ICollection<int> VerseIds { get; set; }
    }
}