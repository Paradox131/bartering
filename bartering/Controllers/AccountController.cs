using Microsoft.AspNetCore.Mvc;

namespace bartering.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
