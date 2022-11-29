using com.itransition.task3.Controllers;
using com.itransition.task3.Models;
using com.itransition.task3.Models.UserModel;
using com.itransition.task3.ViewModels;
using com.itransition.task3.ViewModels.Management;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace com.itransition.task3.Controllers
{
    public class ManagementController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public ManagementController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Users(int page = 1)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            int pageSize = 10;
            var users = _userManager.Users.Where(u => u.Status != Status.Deleted).ToList();
            var count = users.Count();
            var items = users.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            ManagementViewModel managementViewModel = new ManagementViewModel(items, pageViewModel);
            return user.Status == Status.Active ? View(managementViewModel) : await Logout();
        }

        [HttpPost]
        public async Task<IActionResult> ManageSelectedUser(string[] selectedEmails, string actionTitle)
        {
            foreach (var email in selectedEmails)
            {
                var user = await SetStatusToSelectUser(
                    Enum.Parse<ActionTitle>(actionTitle), email);
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Error while manage users!");
                }
            }
            return IsUserShouldBeUnauthenticated(selectedEmails, actionTitle)
                ? await Logout() : RedirectToAction("Users");
        }

        private bool IsUserShouldBeUnauthenticated(string[] selectedEmails, string actionTitle)
        {
            return selectedEmails.Contains(User.Identity?.Name) && actionTitle != ActionTitle.Unblock.ToString();
        }
        
        private async Task<User> SetStatusToSelectUser(ActionTitle actionTitle, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            switch (actionTitle)
            {
                case ActionTitle.Delete:
                    user.Status = Status.Deleted;
                    break;
                case ActionTitle.Unblock:
                    user.Status = Status.Active;
                    break;
                case ActionTitle.Block:
                    user.Status = Status.Blocked;
                    break;
            }
            return user;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}