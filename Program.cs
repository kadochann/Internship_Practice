using BookStoresWebAPI.Data;
using BookStoresWebAPI.Models;
using BookStoresWebAPI.Models.Identity;
using BookStoresWebAPI.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using System.Globalization;



var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders(); // Clear default logging providers
builder.Logging.AddDebug();// Add debug logging
builder.Logging.AddEventSourceLogger(); // Add event source logging

Log.Logger = new LoggerConfiguration() //Serilog configuration - appsetings.json ike entegre
    .ReadFrom.Configuration(builder.Configuration)//JSON'dan oku
    .Enrich.FromLogContext() // Log context bilgilerini ekle
    .CreateLogger();

builder.Host.UseSerilog();// Use Serilog for logging

//other services

builder.Services.AddDbContext<BookStoresDbContext>(options =>// Add this line to register your DbContext
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthorization();

builder.Services.AddIdentity<AppUser, AppRole>() //these are needed to implement register and login
.AddEntityFrameworkStores<BookStoresDbContext>()
.AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Account/AccessDenied";
});


builder.Services.AddControllersWithViews();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    //seed the users and the roles
    await RoleSeeder.SeedRoleAsync(services); //each time the progrma starts, the rolles are controlled
    await UserSeeder.SeedAdminAsync(scope.ServiceProvider, loggerFactory);
    //and add the roles if missing
}



// Configure the HTTP request pipeline (middleware)
if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    // Add static files middleware - THIS IS CRUCIAL for serving images
    app.UseStaticFiles();

    app.UseRouting();

    // Authentication and Authorization should come after UseRouting
    app.UseAuthentication();
    app.UseAuthorization();

    // Keep your existing static assets mapping
    app.MapStaticAssets();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();

    app.Run();
