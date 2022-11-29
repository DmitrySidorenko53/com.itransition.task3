using com.itransition.task3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace com.itransition.task3.Controllers {
    public class ManagementController : Controller {
        private readonly UserManager<User> _userManager;

        public ManagementController(UserManager<User> userManager) {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Users() {
            var users = _userManager.Users.Where(u => u.Status != Status.Deleted).ToList();
            return View(users);
        }

        [HttpPost]
        public IActionResult ManageSelectedUser(FormCollection coll) {

            return RedirectToAction("Users");
        }
    }
}
