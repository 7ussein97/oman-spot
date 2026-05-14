using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PhotoSpotOman.Controllers
{
    public class LogOutController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<LogOutController> _logger;

        public LogOutController(IAuthenticationService authenticationService, ILogger<LogOutController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }
        [Authorize(Policy = "AnyRole")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _authenticationService.SignOutAsync(HttpContext, CookieAuthenticationDefaults.AuthenticationScheme, new AuthenticationProperties());
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError($"LogOutLogout: {ex.Message}");
                return RedirectToAction("Index", "Home");
            }

        }
    }
}
