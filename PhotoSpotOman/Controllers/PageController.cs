using Microsoft.AspNetCore.Mvc;

namespace PhotoSpotOman.Controllers
{
    public class PageController : Controller
    {
        public IActionResult Forbidden()
        {
            return View();
        }
    }
}


