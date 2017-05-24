using System.Collections.Generic; 
using System.Threading.Tasks;
using IronRod.Models;

namespace IronRod.Data
{
    public interface ITopicRepository
    {
        Task<bool> SaveChangesAsync();

        // TOPICS 
        IEnumerable<Topic> GetTopicsByUser(string username);
        Topic GetTopicById(int id);
        bool AddTopic(Topic topic); 
        void RemoveTopic(Topic topic); 
        void EditTopic(Topic topic);

        // PASSAGE TOPICS
        void AddPassageTopic(PassageTopic passagetopic);
        IEnumerable<Topic> GetTopicsByPassage(Passage passage);
        IEnumerable<Passage> GetPassagesByTopic(Topic topic);
        PassageTopic GetPassageTopic(Passage passage, Topic topic);
        void RemovePassageTopic(PassageTopic passagetopic);
        
        // REVIEW  
        IEnumerable<ReviewTopicViewModel> GetReviewTopicsByUser(string username);
        IEnumerable<Passage> GetReviewPassagesByTopic(int id);

        // BACKUP 
        void AddTopics(IEnumerable<Topic> topics);
    }
}