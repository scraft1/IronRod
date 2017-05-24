using Microsoft.AspNetCore.Mvc;
using IronRod.Data;

namespace IronRod.Controllers.Web
{
    public class HomeController : Controller
    {
        private IPassageRepository _repository; 
        public HomeController(IPassageRepository repository){
            _repository = repository; 
        }
        public IActionResult Index()
        {
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
