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

        public void SaveChanges(){
            _context.SaveChanges();
        }

        // PASSAGES 
        public IEnumerable<Passage> GetAllPassages(){
            _logger.LogInformation("Gettting all passages from the database"); 
            return _context.Passages.ToList();
        }
        public IEnumerable<Passage> GetReviewPassages(){
            return _context.Passages.Where(p => p.DatePassed.AddDays(p.Level) <= DateTime.Today).ToList();
        }
        public Passage GetPassageById(int id){
            return _context.Passages.Include(p => p.Verses).SingleOrDefault(p => p.ID == id);
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
            // _context.SaveChanges(); // only save when passage is added ?? 
        }


        // TOPICS 
        public IEnumerable<Topic> GetAllTopics(){
            return _context.Topics.ToList();
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

    }
}