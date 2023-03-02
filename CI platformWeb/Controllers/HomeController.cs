using CI_platformWeb.Models;
using mainProjectsData.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CI_platformWeb.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        private readonly CIPlatformContext _db;

        public HomeController(CIPlatformContext db)
        {
            _db = db;
        }

        //public IActionResult Index()
        //{

        //    IEnumerable<User> objUserData = _db.Users;
        //    return View(objUserData);
        //}

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult NoMissionFound()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MissionDetail()
        {
            return View();
        }
        public IActionResult MissionListing()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}