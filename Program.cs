using BookStoresWebAPI.Data;
using BookStoresWebAPI.Models;
using BookStoresWebAPI.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Globalization;

var cultureInfo = new CultureInfo("tr-TR");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
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
// Add this line to register your DbContext
builder.Services.AddDbContext<BookStoresDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddAuthorization();

builder.Services.AddIdentity<AppUser, AppRole>()
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



builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline (middleware)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

