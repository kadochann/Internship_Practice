using BookStoresWebAPI.Data;
using BookStoresWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoresWebAPI.Controllers
{
    public class ItemsController : Controller
{
        private readonly BookStoresDbContext _context;   // Injecting the DbContext to access the database
        public ItemsController(BookStoresDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
    {
        return View();
    }
        public async Task<IActionResult> Overview()
        {
            var books = await _context.Books.ToListAsync(); // Fetch all books from the database asynchronously

            return View(books); // Pass the list of books to the view
        }
        public IActionResult Create()
        {
            return View(); // Return the view for creating a new book
        }

        [HttpPost] //this attribute indicates that this method will handle POST requests
        [ValidateAntiForgeryToken] // This attribute helps prevent CSRF attacks by validating the request
        public async Task<IActionResult> Create([Bind("Id, Type, Price")] Book book)
        {
            if (ModelState.IsValid) //Check if the model state is valid
            {
                _context.Add(book); // Add the book object to the context
                await _context.SaveChangesAsync(); // Save changes to the database asynchronously
                return RedirectToAction("Overview"); // Redirect to Index action after successful creation
            } // If the model state is not valid,
            return View(book); // return the view with the book object to show validation errors
        }


    }
}
