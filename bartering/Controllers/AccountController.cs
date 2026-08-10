using bartering.Models;
using bartering.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace bartering.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _users;
        private readonly SignInManager<User> _signIn;
        public AccountController(UserManager<User> users, SignInManager<User> signIn)
        {
            _users = users;
            _signIn = signIn;
        }
        [HttpGet]
        public IActionResult Register() => View(new RegisterViewModel());
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                DisplayName = model.DisplayName.Trim(),
                Location = model.Location?.Trim()
            };
            var result = await _users.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(model);
            }
            await _signIn.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
                return View(model);
            var result = await _signIn.PasswordSignInAsync(
                model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
                return RedirectToLocal(returnUrl);
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(model);
        }
        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signIn.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }
    }
}
