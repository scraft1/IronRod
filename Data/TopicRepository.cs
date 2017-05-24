using System;
using System.Collections.Generic;
using System.Linq; 
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using IronRod.Models;

namespace IronRod.Data
{
    public class TopicRepository : ITopicRepository
    {
        private PassagesDbContext _context;
        private ILogger<PassageRepository> _logger;
        public TopicRepository(PassagesDbContext context, ILogger<PassageRepository> logger){
            _context = context;
            _logger = logger; 
        }

        public async Task<bool> SaveChangesAsync(){
            return (await _context.SaveChangesAsync()) > 0;
        }

        // TOPICS 
        public IEnumerable<Topic> GetTopicsByUser(string username){
            return _context.Topics.Where(t => t.UserName == username)
                                    .OrderBy(t => t.Title).ToList();
        }
        public Topic GetTopicById(int id){
            return _context.Topics.SingleOrDefault(t => t.ID == id);
        }
        public bool AddTopic(Topic topic){
            var taken = _context.Topics.FirstOrDefault(t => (t.UserName == topic.UserName) 
                                                        && (t.Title == topic.Title));
            if(taken == null){
                _context.Topics.Add(topic);
                return true;
            }  
            return false;
        }
        public void RemoveTopic(Topic topic){
            _context.Topics.Remove(topic); 
        }
        public void EditTopic(Topic topic){
            var t = GetTopicById(topic.ID);
            t.Title = topic.Title;
        }

        // PASSAGE TOPICS 
        public void AddPassageTopic(PassageTopic passagetopic){
            _context.PassageTopics.Add(passagetopic);
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
            // TODO: change this to single or default
            return _context.PassageTopics.FirstOrDefault(pt => pt.Passage == passage 
                                                            && pt.Topic == topic);
        }
        public void RemovePassageTopic(PassageTopic passagetopic){
            _context.PassageTopics.Remove(passagetopic);
        }

        // REVIEW 
        public IEnumerable<ReviewTopicViewModel> GetReviewTopicsByUser(string username) {
            IEnumerable<PassageTopic> pts = _context.PassageTopics.Where(pt => pt.Passage.UserName == username
                                                && pt.Passage.DatePassed.AddDays(pt.Passage.Level) <= DateTime.Today).ToList();
            IEnumerable<Topic> topics = GetTopicsByUser(username);
            List<ReviewTopicViewModel> reviewTopics = new List<ReviewTopicViewModel>();
            foreach(Topic topic in topics) {
                int count = pts.Where(pt => pt.Topic == topic).ToList().Count;
                if(count > 0) {
                    reviewTopics.Add(new ReviewTopicViewModel(topic.ID,topic.Title,count));
                }
            }
            return reviewTopics;
        }

        public IEnumerable<Passage> GetReviewPassagesByTopic(int id) {
            return _context.PassageTopics.Where(pt => pt.Topic.ID == id && 
                                        pt.Passage.DatePassed.AddDays(pt.Passage.Level) <= DateTime.Today)
                                        .Select(pt => pt.Passage).ToList();
        }

        // BACKUP
        public void AddTopics(IEnumerable<Topic> topics){
            _context.Topics.AddRange(topics);
        }
    }
}