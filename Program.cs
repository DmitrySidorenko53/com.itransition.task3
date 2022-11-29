using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using com.itransition.task3.Models;
using com.itransition.task3.Models.UserModel;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(opts =>
opts.UseSqlServer(connection));

builder.Services.AddIdentity<User, IdentityRole>(
    opts => {
        opts.Password.RequiredLength = 1;
        opts.Password.RequireNonAlphanumeric = false;
        opts.Password.RequireLowercase = false;
        opts.Password.RequireUppercase = false;
        opts.Password.RequireDigit = false;
        opts.User.RequireUniqueEmail = true;
    })
.AddEntityFrameworkStores<ApplicationContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if(!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => {
    endpoints.MapControllerRoute(
    name: "register",
    pattern: "{controller=Account}/{action=Register}");
});

app.UseEndpoints(endpoints => {
    endpoints.MapControllerRoute(
    name: "login",
    pattern: "{controller=Account}/{action=Login}");
});

app.UseEndpoints(endpoints => {
    endpoints.MapControllerRoute(
    name: "users",
    pattern: "{controller=Management}/{action=Users}");
});
app.Run();
