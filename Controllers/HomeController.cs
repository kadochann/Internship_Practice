using BookStoresWebAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using BookStoresWebAPI.Models;
using AspNetCoreGeneratedDocument;
using System.Threading.Tasks;

namespace BookStoresWebAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookStoresDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(BookStoresDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Index action in HomeController was accessed.");
            var item = await _context.Books.ToListAsync();
            return View(item);
        }

        public IActionResult Error()
        {
            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(model);
        }
    }

}
