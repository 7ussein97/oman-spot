using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoSpotOman.Data;
using PhotoSpotOman.Models;

namespace PhotoSpotOman.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class FeedbacksController : Controller
    {
        private readonly SpotContext _context;
        private readonly ILogger<FeedbacksController> _logger;

        public FeedbacksController(SpotContext context, ILogger<FeedbacksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Feedbacks
        public async Task<IActionResult> Index(int? spotId)
        {
            var query = _context.Comments
                .Include(c => c.User)
                .Include(c => c.Spot)
                .AsQueryable();

            if (spotId.HasValue)
            {
                query = query.Where(c => c.SpotId == spotId);
            }

            var comments = await query
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            ViewBag.SpotId = spotId;
            ViewBag.Spots = await _context.Spots.OrderBy(s => s.Name).ToListAsync();

            return View(comments);
        }

        // POST: Feedbacks/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Comment deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}


