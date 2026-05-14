using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoSpotOman.Data;
using PhotoSpotOman.Models;
using System.Security.Claims;

namespace PhotoSpotOman.Controllers
{
    [Authorize(Policy = "AnyRole")]
    public class LikeController : Controller
    {
        private readonly SpotContext _context;
        private readonly ILogger<LikeController> _logger;

        public LikeController(SpotContext context, ILogger<LikeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // POST: Like/Toggle/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Toggle(int spotId)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.Name));

                var existingLike = await _context.Likes
                    .FirstOrDefaultAsync(l => l.SpotId == spotId && l.UserId == userId);

                if (existingLike != null)
                {
                    // Unlike
                    _context.Likes.Remove(existingLike);
                    await _context.SaveChangesAsync();

                    var likeCount = await _context.Likes.CountAsync(l => l.SpotId == spotId);
                    return Json(new { success = true, liked = false, count = likeCount });
                }
                else
                {
                    // Like
                    var like = new Like
                    {
                        SpotId = spotId,
                        UserId = userId
                    };

                    _context.Likes.Add(like);
                    await _context.SaveChangesAsync();

                    var likeCount = await _context.Likes.CountAsync(l => l.SpotId == spotId);
                    return Json(new { success = true, liked = true, count = likeCount });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"LikeToggle: {ex.Message}");
                return Json(new { success = false, message = "An error occurred while toggling the like." });
            }
        }
    }
}


