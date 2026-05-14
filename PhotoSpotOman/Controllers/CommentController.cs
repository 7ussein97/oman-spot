using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoSpotOman.Data;
using PhotoSpotOman.Models;
using System.Security.Claims;

namespace PhotoSpotOman.Controllers
{
    [Authorize(Policy = "AnyRole")]
    public class CommentController : Controller
    {
        private readonly SpotContext _context;
        private readonly ILogger<CommentController> _logger;

        public CommentController(SpotContext context, ILogger<CommentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // POST: Comment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int spotId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return Json(new { success = false, message = "Comment cannot be empty." });
            }

            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.Name));

                var comment = new Comment
                {
                    SpotId = spotId,
                    UserId = userId,
                    Content = content,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                // Load user info for the response
                await _context.Entry(comment)
                    .Reference(c => c.User)
                    .LoadAsync();

                return Json(new
                {
                    success = true,
                    message = "Comment added successfully!",
                    comment = new
                    {
                        id = comment.Id,
                        content = comment.Content,
                        createdAt = comment.CreatedAt.ToString("MMM dd, yyyy HH:mm"),
                        userName = comment.User?.Name
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"CommentCreate: {ex.Message}");
                return Json(new { success = false, message = "An error occurred while adding the comment." });
            }
        }

        // POST: Comment/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var comment = await _context.Comments
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (comment == null)
                {
                    return Json(new { success = false, message = "Comment not found." });
                }

                var userId = int.Parse(User.FindFirstValue(ClaimTypes.Name));
                var isAdmin = User.IsInRole("Admin");

                // Only allow deleting own comments unless admin
                if (!isAdmin && comment.UserId != userId)
                {
                    return Json(new { success = false, message = "You can only delete your own comments." });
                }

                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Comment deleted successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"CommentDelete: {ex.Message}");
                return Json(new { success = false, message = "An error occurred while deleting the comment." });
            }
        }
    }
}


