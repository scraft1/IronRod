using System.Collections.Generic; 
using IronRod.Models;

namespace IronRod.Data
{
    public interface IPassagesRepository
    {
        // PASSAGES 
        IEnumerable<Passage> GetAllPassagesByUser(string username);
        IEnumerable<Passage> GetReviewPassagesByUser(string username);
        Passage GetPassageById(int id);
        void Pass(Passage passage);
        void RemovePassage(Passage passage);
        int CountTotalVerses();
        void AddPassage(Passage passage);
        void AddPassageVerse(PassageVerse pv);
        IEnumerable<int> GetTakenVerseIds(string username, int chapterid);
        IEnumerable<int> GetTakenSMVerseIds(string username, IEnumerable<int> ids);

        // TOPICS 
        IEnumerable<Topic> GetTopicsByUser(string username);
        Topic GetTopicById(int id);
        void AddTopic(Topic topic); 
        void RemoveTopic(Topic topic); 
        void EditTopic(Topic topic);
    }
}