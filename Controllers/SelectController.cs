using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using IronRod.Data;

namespace IronRod.Controllers
{
    public class SelectController : Controller
    {
        private IScripturesRepository _repository; 
        public SelectController(IScripturesRepository repository){
            _repository = repository; 
        }
        public IActionResult Volumes()
        {
            var volumes = _repository.GetVolumes();
            return View(volumes);
        }
        public IActionResult Books(int id)
        {
            var volume = _repository.GetVolumeById(id);
            if(volume == null) return View("Error");
            
            var books = _repository.GetBooks(id);
            if(volume.title == "Doctrine and Covenants"){
                return RedirectToAction("Chapters", new {id = books.First().id}); // 82 
            } 
            ViewData["Volume"] = volume.title;
            return View(books);
        }
        public IActionResult Chapters(int id)
        {
            var book = _repository.GetBookById(id);
            if(book == null) return View("Error");
            
            ViewData["Book"] = book.title; 
            if(book.title == "Doctrine and Covenants") ViewData["Chapter"] = "Section"; 
            else ViewData["Chapter"] = "Chapter";

            var chapters = _repository.GetChapters(id); 
            return View(chapters);
        }
        public IActionResult Verses(int id)
        {
            var chapter = _repository.GetChapterById(id);  
            if(chapter == null) return View("Error");
            var book = _repository.GetBookById(chapter.book_id);

            ViewData["Chapter"] = book.title + " " + chapter.chapter_number; 
            var verses = _repository.GetVerses(chapter.id); 
            return View(verses);
        }
    }
}
