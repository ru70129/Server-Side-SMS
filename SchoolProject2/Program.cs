using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SMS.Repositories;
using SMS.Models;
using SMS.Utilities;
using Microsoft.AspNetCore.Identity.UI.Services;
using SMS.Services;

var builder = WebApplication.CreateBuilder(args);

// Connection string
var connectionString = builder.Configuration.GetConnectionString("SchoolManagementSystem2ContextConnection")
    ?? throw new InvalidOperationException("Connection string not found.");

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Custom services
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IStudentService, StudentService >();

// MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// 🔹 IMPORTANT: seed DB BEFORE routing
//SeedDatabase();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//  AREA ROUTING
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();

//  Local function
//void SeedDatabase()
//{
 //   using var scope = app.Services.CreateScope();
 //   var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
 //   dbInitializer.Initialize();

