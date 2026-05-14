using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoSpotOman.Data;
using PhotoSpotOman.Models;
using System.Diagnostics;

namespace PhotoSpotOman.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SpotContext _context;

        public HomeController(ILogger<HomeController> logger, SpotContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get top 6 approved spots ordered by likes
            var popularSpots = await _context.Spots
                .Include(s => s.Images)
                .Include(s => s.Likes)
                .Include(s => s.Category)
                .Where(s => s.Status == "approved")
                .OrderByDescending(s => s.Likes.Count)
                .ThenByDescending(s => s.Id)
                .Take(6)
                .ToListAsync();

            ViewBag.PopularSpots = popularSpots;
            return View();
        }

    
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
