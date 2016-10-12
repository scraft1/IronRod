using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore; 
using Microsoft.Extensions.Logging;
using IronRod.Data;
using IronRod.Models; 

namespace IronRod.Controllers
{
    [Authorize]
    public class PassagesController : Controller
    {
        private IPassagesRepository _repository; 
        private IScripturesRepository _scriptures; 
        private IScriptureMasteryRepository _smrepo;
        private ILogger<PassagesController> _logger; 
        public PassagesController(IPassagesRepository repository, 
                                IScripturesRepository scriptures, 
                                IScriptureMasteryRepository smrepo,
                                ILogger<PassagesController> logger)
        {
            _repository = repository; 
            _scriptures = scriptures;
            _smrepo = smrepo; 
            _logger = logger;
        }
        public IActionResult List(){
            try {
                var passages =  _repository.GetAllPassagesByUser(this.User.Identity.Name);
                return View(passages);  
            } catch (Exception ex){
                _logger.LogError($"Failed to get list of passages: {ex.Message}");
                return View("Error");
            }
        }
        public IActionResult Detail(int id){
            var passage = _repository.GetPassageById(id); 
            if(passage == null) return View("Error"); 

            var passagetopics = _repository.GetTopicsByPassage(passage);
            var alltopics = _repository.GetTopicsByUser(this.User.Identity.Name);
            ViewData["PassageTopics"] = passagetopics; 
            ViewData["AllTopics"] = alltopics;

            return View(passage);
        }
        [HttpPost] 
        public IActionResult Delete(int id){
            var passage = _repository.GetPassageById(id);
            if(passage == null) return View("Error"); 
            
            _repository.RemovePassage(passage); 
            return RedirectToAction("List"); 
        }
        public IActionResult Create(CreatePassageModel cpm){
            var verseids = cpm.GetVerseIds();
            var versenums = _scriptures.GetVerseNumsByIds(verseids).ToList();
            var title = cpm.ParseReference(versenums); 
            return RedirectToAction("AddPassage", new {
                title = title,
                verseids = verseids
            }); 
        }
        public IActionResult CreateSM(int smset){
            var verseids = _smrepo.GetVerseIdsBySet(smset);
            var title = _smrepo.GetSetById(smset).title;

            return RedirectToAction("AddPassage", new {
                title = title,
                verseids = verseids
            });
        }
        public IActionResult AddPassage(string title, List<int> verseids){
            var passage = new Passage();
            passage.UserName = User.Identity.Name;
            passage.Title = title;
            try{
                foreach(var id in verseids){
                    var verse = _scriptures.GetVerseById(id);
                    if(verse == null) return View("Error"); 
                    var pv = new PassageVerse(passage, verse);
                    _repository.AddPassageVerse(pv);
                } 
            } catch(Exception ex){
                _logger.LogError($"Failed to create new passage: {ex.Message}");
                return View("Error");
            }
            _repository.AddPassage(passage);
            return RedirectToAction("List"); 
        }

        public IActionResult AddTopic(int id, int topicid){
            var passage = _repository.GetPassageById(id);
            var topic = _repository.GetTopicById(topicid);
            if(passage == null || topic == null) return View("Error"); 

            var pt = new PassageTopic(passage, topic);
            _repository.AddPassageTopic(pt);

            return RedirectToAction("Detail", new {id = id});
        }

        public IActionResult RemovePassageTopic(int id, int topicid){
            var passage = _repository.GetPassageById(id);
            var topic = _repository.GetTopicById(topicid);
            if(passage == null || topic == null) return View("Error"); 
            
            var passagetopic = _repository.GetPassageTopic(passage, topic);
            _repository.RemovePassageTopic(passagetopic);
            
            return RedirectToAction("Detail", new {id = id});
        }
    }
}