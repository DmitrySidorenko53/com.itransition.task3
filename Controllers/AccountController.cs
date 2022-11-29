using com.itransition.task3.Models;
using com.itransition.task3.Models.UserModel;
using com.itransition.task3.ViewModels;
using com.itransition.task3.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace com.itransition.task3.Controllers {
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
            var user = createUserFromModel(model);
            var result = await _userManager.CreateAsync(
               user, model.Password
                );
            if(!result.Succeeded) {
                foreach(var error in result.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Users", "Management");
        }

        private User createUserFromModel(RegisterViewModel model) {
            return new User {
                Email = model.Email,
                UserName = model.Email,
                FullName = model.FullName,
                RegisterDate = DateTime.Now,
                LastLoginDate = DateTime.Now,
                Status = Status.Active
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model) {
            if(!ModelState.IsValid) {
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(
                model.Email, model.Password, false, false
                );
            if(!result.Succeeded) {
                ModelState.AddModelError(string.Empty, "Incorrect password or email!");
            }
            return await UpdateWhileLogin(model);
        }
        
        private async Task<IActionResult> UpdateWhileLogin(LoginViewModel model) {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if((!ModelState.IsValid) || !IsUserAvailableToLogin(user)) {
                return View(model);
            }
            user.LastLoginDate = DateTime.Now;
            var result = await _userManager.UpdateAsync(user);
            if(!result.Succeeded) {
                ModelState.AddModelError(string.Empty, "Error while updating user!");
            }
            return !string.IsNullOrEmpty(model.ReturnUrl)
                && Url.IsLocalUrl(model.ReturnUrl) ?
                Redirect(model.ReturnUrl) : RedirectToAction("Users", "Management");
        }

        private bool IsUserAvailableToLogin(User user) {
            return (user.Status != Status.Deleted) && (user.Status != Status.Blocked);
        }
    }
}
