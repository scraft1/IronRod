using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using Microsoft.Extensions.Logging;
using IronRod.Data;
using IronRod.Models; 

namespace IronRod.Controllers
{
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
                var passages =  _repository.GetAllPassages();
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
            var verses = cpm.GetVerseIds(); // split, sort, and convert to int 
            
            var passage = new Passage();
            // _context.Add(passage);  
            // _context.SaveChanges(); 
            
            List<int> verseNums = new List<int>();

            foreach(var id in verses){
                var verse = _scriptures.GetVerseById(id);
                if(verse == null) return View("Error"); 
 
                var pv = new PassageVerse{
                    VerseID=id, 
                    Passage=passage, // need to save first ?? 
                    VerseNumber=verse.verse_number,
                    VerseText=verse.verse_text
                };
                _repository.AddPassageVerse(pv);
                verseNums.Add(pv.VerseNumber); 
            }
            passage.Title = cpm.ParseReference(verseNums); 
            // add passage here ?? 
            _repository.AddPassage(passage);
            return RedirectToAction("List"); 
        }

        
    }
}