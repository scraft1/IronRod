using System;
using System.Collections.Generic;
using System.Linq; 
using Microsoft.EntityFrameworkCore; 
using Microsoft.Extensions.Logging;
using IronRod.Models;

namespace IronRod.Data
{
    public class PassagesRepository : IPassagesRepository
    {
        private PassagesDbContext _context;
        private ILogger<PassagesRepository> _logger;
        public PassagesRepository(PassagesDbContext context, ILogger<PassagesRepository> logger){
            _context = context;
            _logger = logger; 
        }

        // PASSAGES 
        public IEnumerable<Passage> GetAllPassagesByUser(string username){
            _logger.LogInformation("Gettting all passages from the database"); 
            return _context.Passages.Where(p => p.UserName == username).ToList();
        }
        public IEnumerable<Passage> GetReviewPassagesByUser(string username){
            return _context.Passages.Where(p => p.UserName == username)
            .Where(p => p.DatePassed.AddDays(p.Level) <= DateTime.Today).ToList();
        }
        public Passage GetPassageById(int id){
            return _context.Passages.Include(p => p.Verses).Include(p => p.PassageTopics)
                                    .SingleOrDefault(p => p.ID == id);
        }
        public void Pass(Passage passage){
            passage.Passed();
            _context.SaveChanges(); 
        }
        public void RemovePassage(Passage passage){
            _context.Passages.Remove(passage); 
            _context.SaveChanges();
        }
        public int CountTotalVerses(){
            return _context.PassageVerses.ToList().Count; 
        }
        public void AddPassage(Passage passage){
            _context.Passages.Add(passage);
            _context.SaveChanges(); 
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
        

        // TOPICS 
        public IEnumerable<Topic> GetTopicsByUser(string username){
            return _context.Topics.Where(t => t.UserName == username).ToList();
        }
        public Topic GetTopicById(int id){
            return _context.Topics.SingleOrDefault(t => t.ID == id);
        }
        public void AddTopic(Topic topic){
            _context.Topics.Add(topic); 
            _context.SaveChanges();
        }
        public void RemoveTopic(Topic topic){
            _context.Topics.Remove(topic); 
            _context.SaveChanges(); 
        }
        public void EditTopic(Topic topic){
            var t = GetTopicById(topic.ID);
            t.Title = topic.Title;
            _context.SaveChanges(); 
        }

        // PASSAGE TOPICS 
        public void AddPassageTopic(PassageTopic passagetopic){
            _context.PassageTopics.Add(passagetopic);
            _context.SaveChanges(); 
        }
        public IEnumerable<Topic> GetTopicsByPassage(Passage passage){
            return _context.PassageTopics.Where(pt => pt.Passage == passage)
                                        .Select(pt => pt.Topic).ToList();
        }
        public IEnumerable<Passage> GetPassagesByTopic(Topic topic){
            return _context.PassageTopics.Where(pt => pt.Topic == topic)
                                        .Select(pt => pt.Passage).ToList();
        }
        public PassageTopic GetPassageTopic(Passage passage, Topic topic){
            return _context.PassageTopics.SingleOrDefault(pt => pt.Passage == passage 
                                                            && pt.Topic == topic);
        }
        public void RemovePassageTopic(PassageTopic passagetopic){
            _context.PassageTopics.Remove(passagetopic);
            _context.SaveChanges();
        }
    }
}