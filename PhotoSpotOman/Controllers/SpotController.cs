using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoSpotOman.Data;
using PhotoSpotOman.Models;
using System.Security.Claims;

namespace PhotoSpotOman.Controllers
{
    public class SpotController : Controller
    {
        private readonly SpotContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<SpotController> _logger;

        public SpotController(SpotContext context, IWebHostEnvironment environment, ILogger<SpotController> logger)
        {
            _context = context;
            _environment = environment;
            _logger = logger;
        }

        // GET: Spot/Index - All spots (Public for approved, Admin/Contributor for all)
        [AllowAnonymous]
        public async Task<IActionResult> Index(string? status, string? search, int? categoryId)
        {
            var query = _context.Spots
                .Include(s => s.User)
                .Include(s => s.Category)
                .Include(s => s.Images)
                .Include(s => s.Likes)
                .Include(s => s.Comments)
                .AsQueryable();

            // If not admin, only show approved spots
            if (!User.IsInRole("Admin"))
            {
                query = query.Where(s => s.Status == "approved");
            }
            // Filter by status (only for admins)
            else if (!string.IsNullOrEmpty(status) && status != "all")
            {
                query = query.Where(s => s.Status == status);
            }

            // Filter by category
            if (categoryId.HasValue)
            {
                query = query.Where(s => s.CategoryId == categoryId);
            }

            // Search
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.Name.Contains(search) || 
                                        s.Description.Contains(search) || 
                                        s.LocationName.Contains(search));
            }

            var spots = await query.OrderByDescending(s => s.Id).ToListAsync();
            var categories = await _context.Categories.ToListAsync();

            ViewBag.Categories = categories;
            ViewBag.Status = status ?? "all";
            ViewBag.Search = search;
            ViewBag.CategoryId = categoryId;

            return View(spots);
        }

        // GET: Spot/MySpot - User's own spots
        [Authorize(Policy = "AnyRole")]
        public async Task<IActionResult> MySpot()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.Name));
            var spots = await _context.Spots
                .Include(s => s.Category)
                .Include(s => s.Images)
                .Include(s => s.Likes)
                .Include(s => s.Comments)
                .Where(s => s.AddedBy == userId)
                .OrderByDescending(s => s.Id)
                .ToListAsync();

            return View(spots);
        }

        // GET: Spot/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spot = await _context.Spots
                .Include(s => s.User)
                .Include(s => s.Category)
                .Include(s => s.Images)
                .Include(s => s.Likes)
                    .ThenInclude(l => l.User)
                .Include(s => s.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (spot == null)
            {
                return NotFound();
            }

            // Check if current user liked this spot
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.Name));
                ViewBag.UserLiked = spot.Likes.Any(l => l.UserId == userId);
            }
            else
            {
                ViewBag.UserLiked = false;
            }

            return View(spot);
        }

        // GET: Spot/Create
        [Authorize(Policy = "AnyRole")]
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;
            return View();
        }

        // POST: Spot/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AnyRole")]
        public async Task<IActionResult> Create([Bind("Name,Description,LocationName,Latitude,Longitude,Region,CategoryId")] Spot spot, List<IFormFile> images)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = int.Parse(User.FindFirstValue(ClaimTypes.Name));
                    spot.AddedBy = userId;
                    spot.Status = "pending"; // New spots start as pending

                    _context.Add(spot);
                    await _context.SaveChangesAsync();

                    // Handle image uploads
                    if (images != null && images.Count > 0)
                    {
                        await UploadImages(spot.Id, images, userId);
                    }

                    TempData["SuccessMessage"] = "Spot created successfully! It will be reviewed by an admin.";
                    return RedirectToAction(nameof(MySpot));
                }
                catch (Exception ex)
                {
                    _logger.LogError($"SpotCreate: {ex.Message}");
                    TempData["ExceptionMessage"] = "An error occurred while creating the spot.";
                }
            }

            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;
            return View(spot);
        }

        // GET: Spot/Edit/5
        [Authorize(Policy = "AnyRole")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spot = await _context.Spots
                .Include(s => s.Images)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (spot == null)
            {
                return NotFound();
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.Name));
            var isAdmin = User.IsInRole("Admin");

            // Only allow editing own spots unless admin
            if (!isAdmin && spot.AddedBy != userId)
            {
                TempData["ExceptionMessage"] = "You can only edit your own spots.";
                return RedirectToAction(nameof(MySpot));
            }

            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;
            return View(spot);
        }

        // POST: Spot/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AnyRole")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,LocationName,Latitude,Longitude,Region,CategoryId,Status,AddedBy")] Spot spot, List<IFormFile> newImages)
        {
            if (id != spot.Id)
            {
                return NotFound();
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.Name));
            var isAdmin = User.IsInRole("Admin");

            // Only allow editing own spots unless admin
            if (!isAdmin && spot.AddedBy != userId)
            {
                TempData["ExceptionMessage"] = "You can only edit your own spots.";
                return RedirectToAction(nameof(MySpot));
            }

            // If not admin, don't allow status change
            if (!isAdmin)
            {
                var existingSpot = await _context.Spots.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
                if (existingSpot != null)
                {
                    spot.Status = existingSpot.Status;
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(spot);
                    await _context.SaveChangesAsync();

                    // Handle new image uploads
                    if (newImages != null && newImages.Count > 0)
                    {
                        await UploadImages(spot.Id, newImages, userId);
                    }

                    TempData["SuccessMessage"] = "Spot updated successfully!";
                    return RedirectToAction(nameof(MySpot));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpotExists(spot.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"SpotEdit: {ex.Message}");
                    TempData["ExceptionMessage"] = "An error occurred while updating the spot.";
                }
            }

            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;
            return View(spot);
        }

        // POST: Spot/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AnyRole")]
        public async Task<IActionResult> Delete(int id)
        {
            var spot = await _context.Spots
                .Include(s => s.Images)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (spot == null)
            {
                return NotFound();
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.Name));
            var isAdmin = User.IsInRole("Admin");

            // Only allow deleting own spots unless admin
            if (!isAdmin && spot.AddedBy != userId)
            {
                TempData["ExceptionMessage"] = "You can only delete your own spots.";
                return RedirectToAction(nameof(MySpot));
            }

            // Delete associated images from file system
            foreach (var image in spot.Images)
            {
                var imagePath = Path.Combine(_environment.WebRootPath, image.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Spots.Remove(spot);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Spot deleted successfully!";
            return RedirectToAction(nameof(MySpot));
        }

        // POST: Spot/Approve/5 (Admin only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Approve(int id)
        {
            var spot = await _context.Spots.FindAsync(id);
            if (spot == null)
            {
                return NotFound();
            }

            spot.Status = "approved";
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Spot approved successfully!";
            return RedirectToAction(nameof(Index));
        }

        // POST: Spot/Reject/5 (Admin only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Reject(int id)
        {
            var spot = await _context.Spots.FindAsync(id);
            if (spot == null)
            {
                return NotFound();
            }

            spot.Status = "rejected";
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Spot rejected.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Spot/DeleteImage/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AnyRole")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _context.SpotImages
                .Include(i => i.Spot)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (image == null)
            {
                return NotFound();
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.Name));
            var isAdmin = User.IsInRole("Admin");

            // Only allow deleting images from own spots unless admin
            if (!isAdmin && image.Spot.AddedBy != userId)
            {
                return Json(new { success = false, message = "You can only delete images from your own spots." });
            }

            // Delete file from file system
            var imagePath = Path.Combine(_environment.WebRootPath, image.ImagePath.TrimStart('/'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _context.SpotImages.Remove(image);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Image deleted successfully!" });
        }

        // Helper method to upload images
        private async Task UploadImages(int spotId, List<IFormFile> images, int userId)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "spots");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            foreach (var image in images)
            {
                if (image.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}_{image.FileName}";
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    var relativePath = $"/uploads/spots/{fileName}";

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    var spotImage = new SpotImage
                    {
                        SpotId = spotId,
                        ImagePath = relativePath,
                        UploadedBy = userId
                    };

                    _context.SpotImages.Add(spotImage);
                }
            }

            await _context.SaveChangesAsync();
        }

        private bool SpotExists(int id)
        {
            return _context.Spots.Any(e => e.Id == id);
        }
    }
}

