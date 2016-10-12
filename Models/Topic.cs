using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 

namespace IronRod.Models
{
    public class Topic
    {
        public int ID { get; set; }
        public string UserName {get; set;}
        [Required]
        public string Title { get; set; }
        public List<PassageTopic> PassageTopics { get; set; }
    }

    public class PassageTopic
    {
        public int ID {get;set;}
        public int PassageID {get;set;}
        public Passage Passage {get;set;}
        public int TopicID {get;set;}
        public Topic Topic {get;set;}
    }
}