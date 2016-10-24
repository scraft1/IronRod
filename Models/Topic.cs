using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 

namespace IronRod.Models
{
    public class Topic
    {
        public int ID { get; set; }
        [Required]
        public string UserName {get; set;}
        [Required]
        public string Title { get; set; }
        public List<PassageTopic> PassageTopics { get; set; }
    }

    public class TopicViewModel {
        [Required]
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
    }

    public class PassageTopic
    {
        public PassageTopic(){}
        public PassageTopic(Passage passage, Topic topic){
            Passage = passage;
            Topic = topic;
        }
        public int ID {get;set;}
        public int PassageID {get;set;}
        [Required]
        public Passage Passage {get;set;}
        public int TopicID {get;set;}
        [Required]
        public Topic Topic {get;set;}
    }
}