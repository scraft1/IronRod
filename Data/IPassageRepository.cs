using System.Collections.Generic; 
using System.Threading.Tasks;
using IronRod.Models;

namespace IronRod.Data
{
    public interface IPassageRepository
    {
        Task<bool> SaveChangesAsync();

        // PASSAGES 
        IEnumerable<Passage> GetAllPassagesByUser(string username);
        IEnumerable<Passage> GetAllPassagesWithoutTopicsByUser(string username);
        IEnumerable<Passage> GetBackupPassages(string username);
        IEnumerable<Passage> GetReviewPassagesByUser(string username);
        Passage GetPassageById(int id);
        void RemovePassage(Passage passage);
        int CountTotalVerses(string username);
        void AddPassage(Passage passage);
        void AddPassageVerse(PassageVerse pv);
        IEnumerable<int> GetTakenVerseIds(string username, int chapterid);
        IEnumerable<int> GetTakenSMVerseIds(string username, IEnumerable<int> ids);

        // BACKUP 
        void RemoveAllDataByUser(string username);
        void AddPassages(IEnumerable<Passage> passages);
    }
}