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
        public int FirstVerse {get;set;} // used for sorting

        public ICollection<PassageVerse> Verses { get; set; }
        public ICollection<PassageTopic> PassageTopics { get; set; }

        // constructor with username 

        public void Passed() {
            if(DatePassed < DateTime.Today || Level == 0){
                Level = Level + 1;
                DatePassed = DateTime.Today;
            }
        }
    }

    public class PassageViewModel
    {
        public int ID {get; set;}
        public string Title {get; set;}
        public int Level {get; set;}
        public DateTime DatePassed {get; set;}
        public int FirstVerse {get; set;}
    }
}