using BookStoresWebAPI.Data;
using Microsoft.EntityFrameworkCore;
//bu model sayesinde oview sayfasýnda kitap ve yazarlarý listeleyebileceðiz
//@model BookStoresWebAPI.ViewModels.BookOverviewViewModel; ile
var builder = WebApplication.CreateBuilder(args);

// Add this line to register your DbContext
builder.Services.AddDbContext<BookStoresDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
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
