using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore; 
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
        public async Task<IActionResult> Passed(int id){
            var passage = _repository.GetPassageById(id);
            if(passage == null) return View("Error");
            passage.Passed();

            if(await _repository.SaveChangesAsync()) return RedirectToAction("List");    
            return BadRequest("Failed to remove the passage");
        }
    }
}