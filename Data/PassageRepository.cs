using System;
using System.Collections.Generic;
using System.Linq; 
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; 
using Microsoft.Extensions.Logging;
using IronRod.Models;

namespace IronRod.Data
{
    public class PassageRepository : IPassageRepository
    {
        private PassagesDbContext _context;
        private ITopicRepository _topics;
        private ILogger<PassageRepository> _logger;
        
        public PassageRepository(PassagesDbContext context, 
                                ITopicRepository topics,
                                ILogger<PassageRepository> logger){
            _context = context;
            _topics = topics;
            _logger = logger; 
        }

        public async Task<bool> SaveChangesAsync(){
            return (await _context.SaveChangesAsync()) > 0;
        }

        // PASSAGES 
        public IEnumerable<Passage> GetAllPassagesByUser(string username){
            //_logger.LogInformation("Gettting all passages from the database"); 
            return _context.Passages.Where(p => p.UserName == username).Include(p => p.PassageTopics)
                                    .OrderByDescending(p => p.DateCreated).ToList();
        }
        public IEnumerable<Passage> GetAllPassagesWithoutTopicsByUser(string username){
            return _context.Passages.Where(p => p.UserName == username && p.PassageTopics.Count == 0).Include(p => p.PassageTopics)
                                    .OrderByDescending(p => p.DateCreated).ToList();
        }
        public IEnumerable<Passage> GetBackupPassages(string username){
            return _context.Passages.Where(p => p.UserName == username)
                                .Include(p => p.Verses).Include(p => p.PassageTopics).ToList();
        }
        public IEnumerable<Passage> GetReviewPassagesByUser(string username){
            return _context.Passages.Where(p => p.UserName == username)
            .Where(p => p.DatePassed.AddDays(p.Level) <= DateTime.Today).ToList();
        }
        public Passage GetPassageById(int id){
            var passage = _context.Passages.Include(p => p.Verses)
                                    .Include(p => p.PassageTopics)
                                        .ThenInclude(pt => pt.Topic)
                                    .SingleOrDefault(p => p.ID == id);
            if(passage != null)
                passage.Verses = passage.Verses.OrderBy(v => v.VerseID).ToList();
            return passage;
        }
        public void RemovePassage(Passage passage){
            _context.Passages.Remove(passage); 
        }
        public int CountTotalVerses(string username){
            return _context.PassageVerses.Where(pv => pv.Passage.UserName == username).ToList().Count;
        }
        public void AddPassage(Passage passage){
            _context.Passages.Add(passage);
            // add passage verses here ?? 
        }
        public void AddPassageVerse(PassageVerse pv){
            _context.PassageVerses.Add(pv);
        }
        public IEnumerable<int> GetTakenVerseIds(string username, int chapterid){
            return _context.PassageVerses.Where(pv => pv.ChapterID == chapterid
                                        && pv.Passage.UserName == username)
                                        .Select(pv => pv.VerseID).ToList();
        }
        public IEnumerable<int> GetTakenSMVerseIds(string username, IEnumerable<int> ids){
            var taken = new List<int>();
            foreach(var id in ids){
                Console.WriteLine(id);
                var pverse = _context.PassageVerses.Where(pv => pv.Passage.UserName == username)
                                                .FirstOrDefault(pv => pv.VerseID == id);
                if(pverse != null) taken.Add(pverse.VerseID); 
            }
            return taken;
        }
        
        // BACKUP
        public void RemoveAllDataByUser(string username){
            _context.Passages.RemoveRange(GetAllPassagesByUser(username));
            _context.Topics.RemoveRange(_topics.GetTopicsByUser(username));
        }
        public void AddPassages(IEnumerable<Passage> passages){
            _context.Passages.AddRange(passages);
        }

    }
}