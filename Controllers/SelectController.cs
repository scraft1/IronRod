using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using IronRod.Data;

namespace IronRod.Controllers
{
    public class SelectController : Controller
    {
        private IPassagesRepository _repository; 
        private IScripturesRepository _scriptures;  
        public SelectController(IPassagesRepository repository, 
            IScripturesRepository scriptures){
            _repository = repository; 
            _scriptures = scriptures;
        }
        public IActionResult Volumes()
        {
            var volumes = _scriptures.GetVolumes();
            return View(volumes);
        }
        public IActionResult Books(int id)
        {
            var volume = _scriptures.GetVolumeById(id);
            if(volume == null) return View("Error");
            
            var books = _scriptures.GetBooks(id);
            if(volume.title == "Doctrine and Covenants"){
                return RedirectToAction("Chapters", new {id = books.First().id}); // 82 
            } 
            ViewData["Volume"] = volume.title;
            return View(books);
        }
        public IActionResult Chapters(int id)
        {
            var book = _scriptures.GetBookById(id);
            if(book == null) return View("Error");
            
            ViewData["Book"] = book.title; 
            if(book.title == "Doctrine and Covenants") ViewData["Chapter"] = "Section"; 
            else ViewData["Chapter"] = "Chapter";

            var chapters = _scriptures.GetChapters(id); 
            return View(chapters);
        }
        public IActionResult Verses(int id)
        {
            var chapter = _scriptures.GetChapterById(id);  
            if(chapter == null) return View("Error");
            var book = _scriptures.GetBookById(chapter.book_id);

            ViewData["Chapter"] = book.title + " " + chapter.chapter_number; 
            ViewData["Taken"] = _repository.GetTakenVerseIds(this.User.Identity.Name, chapter.id);

            var verses = _scriptures.GetVerses(chapter.id); 
            return View(verses);
        }
    }
}
