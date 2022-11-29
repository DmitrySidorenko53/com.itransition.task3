using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using com.itransition.task3.Models;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(opts =>
opts.UseSqlServer(connection));

builder.Services.AddIdentity<User, IdentityRole>(
    opts => {
        opts.Password.RequiredLength = 6;
        opts.Password.RequireNonAlphanumeric = false;
        opts.Password.RequireLowercase = true;
        opts.Password.RequireUppercase = true;
        opts.Password.RequireDigit = true;
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
    name: "logout",
    pattern: "{controller=Account}/{action=Logout}");
});

app.UseEndpoints(endpoints => {
    endpoints.MapControllerRoute(
    name: "users",
    pattern: "{controller=Management}/{action=Users}");
});
app.Run();
