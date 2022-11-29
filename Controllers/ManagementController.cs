using com.itransition.task3.Models;
using com.itransition.task3.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace com.itransition.task3.Controllers {
    public class ManagementController : Controller {
        private readonly UserManager<User> _userManager;

        public ManagementController(UserManager<User> userManager) {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Users(int page = 1)
        {
            int pageSize = 10;
            var users = _userManager.Users.Where(u => u.Status != Status.Deleted).ToList();
            var count = users.Count();
            var items = users.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            ManagementViewModel managementViewModel = new ManagementViewModel(items, pageViewModel);
            
            return View(managementViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ManageSelectedUser(List<User> users, ManageAction action)
        {
            
            return RedirectToAction("Users");
        }

       
     }

    public enum ManageAction
    {
        BLock, Unblock, Delete
    }
}
