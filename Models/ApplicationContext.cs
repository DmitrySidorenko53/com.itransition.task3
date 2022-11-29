using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace com.itransition.task3.Models {
    public class ApplicationContext : IdentityDbContext<User> {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options) {
            Database.EnsureCreated();
        }
    }
}
