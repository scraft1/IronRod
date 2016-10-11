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
        private ILogger<PassagesController> _logger; 
        public PassagesController(IPassagesRepository repository, 
            IScripturesRepository scriptures, ILogger<PassagesController> logger){
            _repository = repository; 
            _scriptures = scriptures;
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
            var verses = cpm.GetVerseIds();
            
            var passage = new Passage(); 
            List<int> verseNums = new List<int>();

            foreach(var id in verses){
                try{
                    var verse = _scriptures.GetVerseById(id);
                    if(verse == null) return View("Error"); 
    
                    var pv = new PassageVerse{
                        VerseID=id, 
                        Passage=passage, 
                        VerseNumber=verse.verse_number,
                        VerseText=verse.verse_text,
                        ChapterID=verse.chapter_id
                    };
                    _repository.AddPassageVerse(pv);
                    verseNums.Add(pv.VerseNumber); 
                } catch(Exception ex){
                    _logger.LogError($"Failed to create new passage: {ex.Message}");
                    return View("Error");
                }
                
            }
            passage.UserName = User.Identity.Name; 
            passage.Title = cpm.ParseReference(verseNums); 
            _repository.AddPassage(passage);
            return RedirectToAction("List"); 
        }

        
    }
}