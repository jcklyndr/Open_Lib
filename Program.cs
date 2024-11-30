using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using OopProject.Data;
using OopProject.Services;
using OopProject.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation(); // For runtime updates

builder.Services.AddDbContext<OpenLibDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Register the generic repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IRepository<Request>, RequestRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();


// Add authentication with multiple schemes
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "UserScheme";
})
.AddCookie("UserScheme", options =>
{
    options.Cookie.Name = "UserAuthCookie";
    options.LoginPath = "/Auth/UserLogin";
    options.LogoutPath = "/Auth/Logout";
})
.AddCookie("AdminScheme", options =>
{
    options.Cookie.Name = "AdminAuthCookie";
    options.LoginPath = "/Admin/AdminLogin"; // Redirects to this page for unauthenticated admins
    options.LogoutPath = "/Admin/AdminLogout";
});




// Enable sessions (optional)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Enable session middleware
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
