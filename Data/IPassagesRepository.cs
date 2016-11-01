using System.Collections.Generic; 
using System.Threading.Tasks;
using IronRod.Models;

namespace IronRod.Data
{
    public interface IPassagesRepository
    {
        Task<bool> SaveChangesAsync();

        // PASSAGES 
        IEnumerable<Passage> GetAllPassagesByUser(string username);
        IEnumerable<Passage> GetPassagesWithVerses(string username);
        IEnumerable<Passage> GetReviewPassagesByUser(string username);
        Passage GetPassageById(int id);
        void RemovePassage(Passage passage);
        int CountTotalVerses(string username);
        void AddPassage(Passage passage);
        void AddPassages(IEnumerable<Passage> passages);
        void AddPassageVerse(PassageVerse pv);
        IEnumerable<int> GetTakenVerseIds(string username, int chapterid);
        IEnumerable<int> GetTakenSMVerseIds(string username, IEnumerable<int> ids);

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

        // BACKUP 
        void RemoveAllPassagesByUser(string username);
    }
}