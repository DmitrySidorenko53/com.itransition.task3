using System.ComponentModel.DataAnnotations;

namespace com.itransition.task3.ViewModels.Account {
    public class LoginViewModel {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        
        public string? ReturnUrl { get; set; }
    }
}
