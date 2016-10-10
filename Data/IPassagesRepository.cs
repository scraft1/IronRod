using System.Collections.Generic; 
using IronRod.Models;

namespace IronRod.Data
{
    public interface IPassagesRepository
    {
        // GENERAL
        void SaveChanges();

        // PASSAGES 
        IEnumerable<Passage> GetAllPassages();
        IEnumerable<Passage> GetReviewPassages();
        Passage GetPassageById(int id);
        void Pass(Passage passage);
        void RemovePassage(Passage passage);
        int CountTotalVerses();
        void AddPassage(Passage passage);
        void AddPassageVerse(PassageVerse pv);

        // TOPICS 
        IEnumerable<Topic> GetAllTopics();
        Topic GetTopicById(int id);
        void AddTopic(Topic topic); 
        void RemoveTopic(Topic topic); 
        void EditTopic(Topic topic);
    }
}