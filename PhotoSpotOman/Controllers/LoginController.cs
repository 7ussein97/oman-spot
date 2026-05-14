using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PhotoSpotOman.Dto;
using PhotoSpotOman.Helper;
using PhotoSpotOman.Models;
using PhotoSpotOman.Data;
using System.Security.Claims;


namespace PhotoSpotOman.Controllers
{
    public class LoginController : Controller
    {
        private readonly SpotContext _context;
        private readonly IAuthenticationService _authenticationService;
        private readonly JWTSettings _jwtSetting;
        private readonly ILogger _logger;
        public LoginController(SpotContext context, IAuthenticationService authenticationService, ILogger<LoginController> logger, IOptions<JWTSettings> jwtSettings)
        {
            _context = context;
            _authenticationService = authenticationService;
            _jwtSetting = jwtSettings.Value;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromForm] UserDto user)
        {

            try
            {
                var IsUser = await _context.Users.Where(x => x.Email == user.Email).FirstOrDefaultAsync();
                if (IsUser != null)
                {
                    if (!HashHelper.VerifyPasswordHash(user.Password, IsUser.Password))
                    {
                        TempData["ExceptionMessage"] = "Invalid Password";
                        return RedirectToAction(nameof(Index));
                    }
                    var authenticationProperties = new AuthenticationProperties
                    {
                        IsPersistent = false,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(50)
                    };
                    await _authenticationService.SignInAsync(
                        HttpContext,
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Name, IsUser.Id.ToString()),
                            new Claim(ClaimTypes.Role, IsUser.Role)
                        }, CookieAuthenticationDefaults.AuthenticationScheme)),
                        authenticationProperties);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["ExceptionMessage"] = "Account not Found";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"LoginSignIn: {ex.Message}");
                TempData["ExceptionMessage"] = "An error occurred while processing your request.";
                return RedirectToAction(nameof(Index));
            }



        }
        [HttpPost]
        public async Task<IActionResult> SignUp([FromForm] User user)
        {
            if (!ModelState.IsValid)
            {
                TempData["ExceptionMessage"] = "Some Entries are Invalid";
                return RedirectToAction(nameof(Index));
            }
            try
            {
                var IsUser = await _context.Users.Where(x => x.Email == user.Email).FirstOrDefaultAsync();
                if (IsUser == null)
                {
                    HashHelper.CreatePasswordHash(user.Password, out var hash);
                    

                    var newUser = new User
                    {
                    
                        Name = user.Name,
                        Email = user.Email,
                        Role= "Contributor",
                        Password = hash,
                    };
                    _context.Users.Add(newUser);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Account created successfully. Please Login";
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    TempData["ExceptionMessage"] = "User Already Has Record In PhotoSpotOman";
                    return RedirectToAction(nameof(Index));
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"LoginSignUp: {ex.Message}");
                TempData["ExceptionMessage"] = "An error occurred while processing your request.";
                return RedirectToAction(nameof(Index));

            }

        }
    }
}
