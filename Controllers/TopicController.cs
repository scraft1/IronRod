using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IronRod.Data;
using IronRod.Models; 

namespace IronRod.Controllers
{
    public class TopicController : Controller
    {
        private IPassagesRepository _repository; 
        public TopicController(IPassagesRepository repository){
            _repository = repository; 
        }
        public IActionResult List(){
            var topics = _repository.GetAllTopics();
            ViewData["Topics"] = topics;  
            return View();
        }
        public IActionResult Detail(int id){
            var topic = _repository.GetTopicById(id);
            if(topic == null) return View("Error");  
            return View(topic);
        }
        [HttpPost] 
        public IActionResult Create(Topic topic){
            if(ModelState.IsValid){
                _repository.AddTopic(topic);
            }
            return RedirectToAction("List"); 
        }
        public IActionResult Edit(int id){
            var topic = _repository.GetTopicById(id);
            if(topic == null) return View("Error"); 
            return View(topic);
        }
        [HttpPost] 
        public IActionResult Edit(Topic topic){
            if(ModelState.IsValid){
                var oldTopic = _repository.GetTopicById(topic.ID);
                if(oldTopic == null) return View("Error");
                _repository.EditTopic(topic);
            }
            return RedirectToAction("Detail", new {id = topic.ID}); 
        }
        [HttpPost] 
        public IActionResult Delete(int id){
            var topic = _repository.GetTopicById(id);
            if(topic == null) return View("Error"); 
            _repository.RemoveTopic(topic);
            return RedirectToAction("List"); 
        }
    }
}