using BookStoresWebAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoresWebAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookStoresDbContext _context;   // Injecting the DbContext to access the database
        public HomeController(BookStoresDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var item= await _context.Books.ToListAsync(); // async/await allows us to wait for the database operation
                                                          // to complete without blocking the thread
            return View(item);// Passing all the data we got from the database to the view
        }
    }
}
