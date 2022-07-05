using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using postgresposhelnah.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using postgresposhelnah.Service;
using System.Linq;
using System.Security.Claims;
namespace postgresposhelnah.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<Appuser> UserManager { get; set; }

        private SignInManager<Appuser> SignInManager { get; set; }
        public AccountController(UserManager<Appuser> userManager, SignInManager<Appuser> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }


        [AllowAnonymous]
        public IActionResult Login() => View();        

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model,string returnUrl)
        {
          Appuser user = await UserManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                await SignInManager.SignOutAsync();
                Microsoft.AspNetCore.Identity.SignInResult result = await SignInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (result.Succeeded)
                {
                    return View("Profile", user);
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Registration() => View();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegisterModel model)
        {
            Appuser user = new Appuser() { 
                UserName = model.UserName,
                Email = model.Email 
            };

            IdentityResult result = await UserManager.CreateAsync(user,model.Password);

            if (result.Succeeded)
            {
                var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                var callBackUrl = Url.Action(
                    "ConfirmEmail",
                    "Account",
                    new { userId = user.Id, code = code },
                    protocol: HttpContext.Request.Scheme);

                EmailConfirmService service = new EmailConfirmService();
                await service.SendEmailAsync(model.Email, "Confirm Your Account", $"Follow to link to confirm your Account <a href='{callBackUrl}'><br/>Link</a>");
                return View("Confirm");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]

        public async Task<IActionResult> ConfirmEmail(string userId,string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }

            var result = await UserManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return View("Login");
            }
            return View("Error");
        }

        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return View("Login");
        }
        
        public async Task<IActionResult> Profile(Appuser user)
        {
            Appuser User = await UserManager.FindByIdAsync(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (User == null) return RedirectToAction("Logout");
            return View(user);
        }
    }
}
