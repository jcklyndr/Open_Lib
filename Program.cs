using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using OopProject.Data;
using OopProject.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation(); // For runtime updates

builder.Services.AddDbContext<OpenLibDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the generic repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Add authentication with multiple schemes
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "UserScheme"; // Default for unauthenticated requests
})
.AddCookie("UserScheme", options =>
{
    options.LoginPath = "/Auth/UserLogin";
    options.LogoutPath = "/Auth/Logout";
})
.AddCookie("AdminScheme", options =>
{
    options.LoginPath = "/Admin/AdminLogin";
    options.LogoutPath = "/Admin/Logout";
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
