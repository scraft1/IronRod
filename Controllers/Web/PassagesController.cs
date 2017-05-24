using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using IronRod.Data;
using IronRod.Models; 

namespace IronRod.Controllers.Web
{
    [Authorize]
    public class PassagesController : Controller
    {
        private IPassageRepository _passages; 
        private ITopicRepository _topics;
        private IScripturesRepository _scriptures; 
        private IScriptureMasteryRepository _smrepo;
        private ILogger<PassagesController> _logger; 
        public PassagesController(IPassageRepository passages, 
                                ITopicRepository topics,
                                IScripturesRepository scriptures, 
                                IScriptureMasteryRepository smrepo,
                                ILogger<PassagesController> logger)
        {
            _passages = passages; 
            _topics = topics;
            _scriptures = scriptures;
            _smrepo = smrepo; 
            _logger = logger;
        }
        [Route("Passages")]
        public IActionResult List(){
            return View();
        }
        public IActionResult Detail(int id){
            var passage = _passages.GetPassageById(id); 
            if(passage == null) return View("Error"); 

            var passagetopics = _topics.GetTopicsByPassage(passage);
            var alltopics = _topics.GetTopicsByUser(this.User.Identity.Name);
            var availabletopics = alltopics.Except(passagetopics).ToList();
            ViewData["PassageTopics"] = passagetopics; 
            ViewData["AvailableTopics"] = availabletopics;

            return View(passage);
        }
        [HttpPost] 
        public async Task<IActionResult> Delete(int id){
            var passage = _passages.GetPassageById(id);
            if(passage == null) return View("Error"); 

            _passages.RemovePassage(passage); 
            if(await _passages.SaveChangesAsync()) return RedirectToAction("List");    
            return BadRequest("Failed to remove the passage");
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
        public async Task<IActionResult> AddPassage(string title, List<int> verseids){
            var passage = new Passage();
            passage.UserName = User.Identity.Name;
            passage.Title = title;
            try{
                foreach(var id in verseids){
                    // if verse already taken by User, redirect to select chapter
                    var verse = _scriptures.GetVerseById(id);
                    if(verse == null) return View("Error"); 
                    var pv = new PassageVerse(passage, verse);
                    _passages.AddPassageVerse(pv);
                }
                passage.FirstVerse = verseids[0];
            } catch(Exception ex){
                _logger.LogError($"Failed to create new passage: {ex.Message}");
                return View("Error");
            }
            _passages.AddPassage(passage);

            if(await _passages.SaveChangesAsync()) return RedirectToAction("Detail", new {id = passage.ID}); 
            return BadRequest("Failed to add the passage");
        }

        public async Task<IActionResult> AddTopic(int id, int topicid){
            var passage = _passages.GetPassageById(id);
            var topic = _topics.GetTopicById(topicid);
            var pt = _topics.GetPassageTopic(passage, topic);
            if(passage == null || topic == null || pt != null) return View("Error"); 

            pt = new PassageTopic(passage, topic);
            _topics.AddPassageTopic(pt);

            if(await _passages.SaveChangesAsync()) return RedirectToAction("Detail", new {id = id});  
            return BadRequest("Failed to add the topic");
        }

        public async Task<IActionResult> RemovePassageTopic(int id, int topicid){
            var passage = _passages.GetPassageById(id);
            var topic = _topics.GetTopicById(topicid);
            if(passage == null || topic == null) return View("Error"); 
            
            var passagetopic = _topics.GetPassageTopic(passage, topic);
            _topics.RemovePassageTopic(passagetopic);
            
            if(await _topics.SaveChangesAsync()) return RedirectToAction("Detail", new {id = id});  
            return BadRequest("Failed to remove the topic");
        }

        [Authorize(Roles = "Privileged")]
        public IActionResult Privileged(){
            var passages = _passages.GetAllPassagesByUser(this.User.Identity.Name);
            return View(passages);
        }

        [Authorize(Roles = "Privileged")]
        [HttpPost]
        public async Task<IActionResult> SetLevel(int id, int level){
            var passage = _passages.GetPassageById(id);
            if(passage == null) return View("Error"); 
            if(level >= 0 && level != passage.Level){
                passage.Level = level;
                await _passages.SaveChangesAsync();  
            }
            return RedirectToAction("List");
        }

        // public FileResult Download(){
        //     var passage = new Passage();
        //     passage.DatePassed = DateTime.Now;
        //     passage.Title = "Test passage";

        //     string json = JsonConvert.SerializeObject(passage);
        //     byte[] toBytes = Encoding.ASCII.GetBytes(json);
        //     var file = new FileContentResult(toBytes,"application/json");
            
        //     return new FileContentResult();
        // }
        // 
    }
}