using System;
using System.Collections.Generic;

namespace IronRod.Models
{
    public class Passage
    {
        public int ID { get; set; }
        public string UserName {get; set;}
        public string Title {get; set;}
        public int Level {get; set;} = 0; 
        public DateTime DatePassed {get; set;} = DateTime.Today; 

        public ICollection<PassageVerse> Verses { get; set; }

        public void Passed() {
            if(DatePassed < DateTime.Today || Level == 0){
                Level = Level + 1;
                DatePassed = DateTime.Today;
            }
        }
    }
}