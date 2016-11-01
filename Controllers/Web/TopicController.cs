using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IronRod.Data;
using IronRod.Models; 

namespace IronRod.Controllers.Web
{
    public class TopicController : Controller
    {
        private IPassagesRepository _repository; 
        public TopicController(IPassagesRepository repository){
            _repository = repository; 
        }
        public IActionResult List(){
            var topics = _repository.GetTopicsByUser(this.User.Identity.Name);
            return View(topics);
        }
        public IActionResult Detail(int id){
            var topic = _repository.GetTopicById(id);
            if(topic == null) return View("Error");

            var passages = _repository.GetPassagesByTopic(topic);
            ViewData["Passages"] = passages;  
            return View(topic);
        }
        [HttpPost] 
        public async Task<IActionResult> Create(string title){
            if(title != null){
                var topic = new Topic(this.User.Identity.Name, title);

                if(_repository.AddTopic(topic)) await _repository.SaveChangesAsync();
                return RedirectToAction("List"); 
            }
            return BadRequest("Failed to add the topic");
        }
        public IActionResult Edit(int id){
            var topic = _repository.GetTopicById(id);
            if(topic == null) return View("Error"); 
            return View(topic);
        }
        [HttpPost] 
        public async Task<IActionResult> Edit(TopicViewModel tvm){
            if(ModelState.IsValid){ // use topic view model (id, title) ?? 
                var topic = _repository.GetTopicById(tvm.ID); // for user ?? 
                if(topic == null) return View("Error");
                topic.Title = tvm.Title;

                if(await _repository.SaveChangesAsync()){
                    return RedirectToAction("Detail", new {id = topic.ID}); 
                }
            }
            return BadRequest("Failed to edit topic"); 
        }
        [HttpPost] 
        public async Task<IActionResult> Delete(int id){
            var topic = _repository.GetTopicById(id);
            if(topic == null) return View("Error"); 
            _repository.RemoveTopic(topic);
            if(await _repository.SaveChangesAsync()) return RedirectToAction("List"); 
            return BadRequest("Failed to remove the topic"); 
        }
    }
}