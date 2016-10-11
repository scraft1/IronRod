using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore; 
using IronRod.Data;
using IronRod.Models; 

namespace IronRod.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private IPassagesRepository _repository; 
        public ReviewController(IPassagesRepository repository){
            _repository = repository; 
        }
        public IActionResult List(){
            var passages = _repository.GetReviewPassagesByUser(this.User.Identity.Name);
            return View(passages); 
        }
        public IActionResult Detail(int id){
            var passage = _repository.GetPassageById(id); 
            if(passage == null) return View("Error");
            return View(passage);
        }
        public IActionResult Passed(int id){
            var passage = _repository.GetPassageById(id);
            if(passage == null) return View("Error");
            _repository.Pass(passage);
            return RedirectToAction("List");
        }
    }
}