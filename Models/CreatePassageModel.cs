using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations; 

namespace IronRod.Models
{
    public class CreatePassageModel
    {
        public string chapter { get; set; }
        public string verses {get; set;}

        public List<int> GetVerseIds() { 
            string[] versesArray = verses.Split(',');
            Array.Sort(versesArray);
            return versesArray.Select(int.Parse).ToList(); 
        }
        public string ParseReference(List<int> verses){
            if(chapter.Length > 22 && chapter.Substring(0,22) == "Doctrine and Covenants"){
                chapter = "D&C"+chapter.Substring(22); 
            }

            string reference = chapter+":"+verses[0].ToString();
            var prev = verses[0]; 
            for(var i = 1; i < verses.Count; i++){
                if(verses[i] == prev + 1){ // verse directly after last verse
                    if(reference.Last() == '-'){ // last character added 
                        if(i == verses.Count - 1){ // last verse 
                            reference += verses[i].ToString(); 
                        }
                    } else { // last character added was a number
                        if(i == verses.Count - 1){
                            reference += "-"+verses[i].ToString(); 
                        } else {
                            reference += "-"; 
                        }
                    }
                } else { // verse skips ahead in chapter 
                    if(reference.Last() == '-'){
                        reference += prev.ToString()+","+verses[i].ToString();
                    } else { // last character added was a number
                        reference += ","+verses[i].ToString();
                    }
                }
                prev = verses[i]; 
            }
            return reference; 
        }
    }
}