using Microsoft.AspNetCore.Identity;

namespace com.itransition.task3.Models {
    public class User : IdentityUser {
        public string FullName { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public Status Status { get; set; }
    }
}
