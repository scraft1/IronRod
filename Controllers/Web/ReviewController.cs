using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using IronRod.Data;
using IronRod.Models; 

namespace IronRod.Controllers.Web
{
    [Authorize]
    public class ReviewController : Controller
    {
        private IPassagesRepository _repository; 
        private ILogger<ReviewController> _logger; 
           
        public ReviewController(IPassagesRepository repository,
                                ILogger<ReviewController> logger){
            _repository = repository;
            _logger = logger; 
        }
        [Route("Review")]
        public IActionResult List(){
            return View(); 
        }
        public IActionResult Detail(int id){
            var passage = _repository.GetPassageById(id); 
            if(passage == null) return View("Error");
            return View(passage);
        }
    }
}