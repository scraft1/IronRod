using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 

namespace IronRod.Models
{
    public class Topic
    {
        public int ID {get;set;}
        [Required]
        public string UserName {get;set;}
        [Required]
        public string Title {get;set;}
        public List<PassageTopic> PassageTopics {get;set;}

        public Topic(){}
        public Topic(string username, string title){
            UserName = username;
            Title = title;
            PassageTopics = new List<PassageTopic>();
        }
    }

    public class TopicViewModel {
        [Required]
        public int ID {get;set;}
        [Required]
        public string Title {get;set;}
    }

    public class ReviewTopicViewModel {
        [Required]
        public int ID {get;set;}
        [Required]
        public string Title {get;set;}
        public int Count {get;set;}
        public ReviewTopicViewModel(int id, string title, int count) {
            ID = id;
            Title = title;
            Count = count;
        }
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