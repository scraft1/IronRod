using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IronRod.Data;

namespace IronRod.Controllers.Web
{
    public class HomeController : Controller
    {
        private IPassagesRepository _repository; 
        public HomeController(IPassagesRepository repository){
            _repository = repository; 
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize] 
        public IActionResult Stats()
        {
            ViewData["Total"] = _repository.CountTotalVerses();
            return View();
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        // public IActionResult Contact()
        // {
        //     ViewData["Message"] = "Your contact page.";

        //     return View();
        // }

        public IActionResult Error()
        {
            return View();
        }
    }
}
