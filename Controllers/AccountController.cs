using com.itransition.task3.Models;
using com.itransition.task3.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers {
    public class AccountController : Controller {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register() {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null) {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model) {
            if(!ModelState.IsValid) {
                return View(model);
            }
            var result = await _userManager.CreateAsync(
                createUserFromModel(model), model.Password
                );
            if(!result.Succeeded) {
                foreach(var error in result.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return RedirectToAction("Login", "Account");
        }

        private User createUserFromModel(RegisterViewModel model) {
            return new User {
                Email = model.Email,
                UserName = model.Email,
                FullName = model.FullName,
                RegisterDate = DateTime.Now
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model) {
            if(!ModelState.IsValid) {
                var errors = ModelState.Values.ToList();
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(
                model.Email, model.Password, false, false
                );
            if(!result.Succeeded) {
                ModelState.AddModelError(string.Empty, "Incorrect password or email");
            }
            return await UpdateWhileLogin(model);
        }

        [HttpPost]
        private async Task<IActionResult> UpdateWhileLogin(LoginViewModel model) {
            User user = await _userManager.FindByEmailAsync(model.Email);
            if((!ModelState.IsValid) || !isUserAvaibleToLogin(user)) {
                return View(model);
            }
            user.LastLoginDate = DateTime.Now;
            user.Status = Status.Active;
            var result = await _userManager.UpdateAsync(user);
            if(!result.Succeeded) {
                foreach(var error in result.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return !string.IsNullOrEmpty(model.ReturnUrl)
                && Url.IsLocalUrl(model.ReturnUrl) ?
                Redirect(model.ReturnUrl) : RedirectToAction("Users", "Management");
        }

        private bool isUserAvaibleToLogin(User user) {
            return (user.Status == Status.Deleted) || (user.Status == Status.Blocked)
                ? false : true;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
