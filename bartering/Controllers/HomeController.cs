using bartering.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using bartering.Services;

namespace bartering.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly IItemService _items;
       // public HomeController(ILogger<HomeController> logger)
     public HomeController(IItemService items)
        {
           // _logger = logger;
            _items = items;
        }
        //public IActionResult Index()
     public async Task<IActionResult> Index()
        {
            var browse = await _items.BrowseAsync(null, null);
            ViewBag.RecentItems = browse.Items.Take(6).ToList();
            return View();
        }
        //public IActionResult Privacy()
       // {
       //     return View();
       // }
        public IActionResult Privacy() => View();
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
           // return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            return View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
        }
    }
}
