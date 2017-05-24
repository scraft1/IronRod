using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IronRod.Data;
using IronRod.Models; 

namespace IronRod.Controllers.Web
{
    public class TopicController : Controller
    {
        private ITopicRepository _topics; 
        public TopicController(ITopicRepository topics){
            _topics = topics; 
        }
        public IActionResult List(){
            var topics = _topics.GetTopicsByUser(this.User.Identity.Name);
            return View(topics);
        }
        public IActionResult Detail(int id){
            var topic = _topics.GetTopicById(id);
            if(topic == null) return View("Error");

            var passages = _topics.GetPassagesByTopic(topic);
            ViewData["Passages"] = passages;  
            return View(topic);
        }
        [HttpPost] 
        public async Task<IActionResult> Create(string title){
            if(title != null){
                var topic = new Topic(this.User.Identity.Name, title);

                if(_topics.AddTopic(topic)) await _topics.SaveChangesAsync();
                return RedirectToAction("List"); 
            }
            return BadRequest("Failed to add the topic");
        }
        public IActionResult Edit(int id){
            var topic = _topics.GetTopicById(id);
            if(topic == null) return View("Error"); 
            return View(topic);
        }
        [HttpPost] 
        public async Task<IActionResult> Edit(TopicViewModel tvm){
            if(ModelState.IsValid){ // use topic view model (id, title) ?? 
                var topic = _topics.GetTopicById(tvm.ID); // for user ?? 
                if(topic == null) return View("Error");
                topic.Title = tvm.Title;

                if(await _topics.SaveChangesAsync()){
                    return RedirectToAction("Detail", new {id = topic.ID}); 
                }
            }
            return BadRequest("Failed to edit topic"); 
        }
        [HttpPost] 
        public async Task<IActionResult> Delete(int id){
            var topic = _topics.GetTopicById(id);
            if(topic == null) return View("Error"); 
            _topics.RemoveTopic(topic);
            if(await _topics.SaveChangesAsync()) return RedirectToAction("List"); 
            return BadRequest("Failed to remove the topic"); 
        }
    }
}