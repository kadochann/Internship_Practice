using BookStoresWebAPI.Data;
using BookStoresWebAPI.Models;
using BookStoresWebAPI.ViewModel;
using BookStoresWebAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace BookStoresWebAPI.Controllers
{
    public class ItemsController : Controller
    {
        private readonly BookStoresDbContext _context;
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(BookStoresDbContext context, ILogger<ItemsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /*public IActionResult Index()
        {
            _logger.LogInformation("Index method called in ItemsController.");
            return View();
        }*/
        public async Task<IActionResult> Overview()
        {
            _logger.LogInformation("Overview method called in ItemsController.");
            var books = await _context.Books
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .ToListAsync();

            var bookVMs = books.Select(book => new BookWithAuthorVM
            {
                BookId = book.BookId,
                Title = book.Title,
                Type = book.Type,
                Price = book.Price,
                AuthorNames = book.BookAuthors
                    .Select(ba => $"{ba.Author.FirstName} {ba.Author.LastName}")
                    .ToList()
            }).ToList();

            return View(bookVMs);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            _logger.LogInformation("Create method called in ItemsController.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(BookCreateViewModel model) //argüman olarak view model çünkü input oraya 
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Model is not valid.");
                return View();
            }
            var book = new Book
            {
                Title = model.Title,
                Type = model.Type,
                Price = model.Price,
                PublishedDate = DateTime.Now,
                Notes = model.Notes,
                PubId = 1 // (örnek, değiştirilebilir)
            };
            _logger.LogInformation("Gelen fiyat değeri: {Price}", model.Price); // Moved outside of the object initializer
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var author = new Author
            {
                FirstName = model.AuthorFirstName,
                LastName = model.AuthorLastName,
            };
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            // 🔗 İlişkiyi kur
            var bookAuthor = new BookAuthor
            {
                BookId = book.BookId,
                AuthorId = author.AuthorId,
                AuthorOrder = 1,
                RoyalityPercentage = 0
            };

            _context.BookAuthors.Add(bookAuthor);
            await _context.SaveChangesAsync();

            TempData["Msg"] = "kitap başarıyla eklendi!";
            return RedirectToAction("Overview");
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
                return NotFound();

            var viewModel = new BookDeleteViewModel
            {
                Id = book.BookId,
                Title = book.Title,
                AuthorName = book.BookAuthors
                    .Select(ba => $"{ba.Author.FirstName} {ba.Author.LastName}")
                    .ToList()
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete_Confirmed(int id)
        {
            _logger.LogInformation("Delete method called in ItemsController for book with ID {Id}.", id); // Log the ID of the book being deleted
            var book = await _context.Books
                .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
                .FirstOrDefaultAsync(b => b.BookId == id);
            if (book == null)
            {
                _logger.LogWarning("Book with ID {Id} not found in Delete method of ItemsController.", id);
                return NotFound();
            }
            //önce bookun authorlarını sil
            _context.BookAuthors.RemoveRange(book.BookAuthors);

            //sonra kitabı sil
            _context.Books.Remove(book);

            await _context.SaveChangesAsync();
            
            return RedirectToAction("Overview");
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            _logger.LogInformation("Detail GET method called for book ID: {Id}", id);

            var book = await _context.Books
                .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                _logger.LogWarning("Book with ID {Id} not found in Details method.", id);
                return NotFound();
            }

            _logger.LogInformation("Book found. ImagePath from database: '{ImagePath}'", book.ImagePath ?? "NULL");

            var firstAuthor = book.BookAuthors.FirstOrDefault()?.Author;
            var viewModel = new BookCreateViewModel
            {
                Id = book.BookId,
                Title = book.Title,
                AuthorFirstName = firstAuthor?.FirstName ?? "",
                AuthorLastName = firstAuthor?.LastName ?? "",
                Notes = book.Notes,
                Price = book.Price ?? 0,
                Type = book.Type,
                ImagePath = book.ImagePath
            };

            _logger.LogInformation("ViewModel created. ImagePath in ViewModel: '{ImagePath}'", viewModel.ImagePath ?? "NULL");


            return View(viewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Detail_edit(BookCreateViewModel model)
        {

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid when updating book.");
                return View(model);
            }

            var book = await _context.Books
                .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
                .FirstOrDefaultAsync(b => b.BookId == model.Id);

            if (book == null) return NotFound();

            // Kitap güncellemesi
            book.Title = model.Title;
            book.Type = model.Type;
            book.Notes = model.Notes;
            book.Price = model.Price;

            // İlgili yazar güncellemesi (sadece ilk yazar için örnek)
            var bookAuthor = book.BookAuthors.FirstOrDefault();
            if (bookAuthor != null)
            {
                var author = await _context.Authors.FindAsync(bookAuthor.AuthorId);
                if (author != null)
                {
                    author.FirstName = model.AuthorFirstName;
                    author.LastName = model.AuthorLastName;
                }
            }

            await _context.SaveChangesAsync();
            TempData["Msg"] = "Kitap başarıyla güncellendi.";
            return RedirectToAction("Overview");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadImage(int id, IFormFile imageFile)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                TempData["Error"] = "Kitap bulunamadı.";
                return RedirectToAction("Detail", new { id });
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{id}.jpg";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await imageFile.CopyToAsync(stream);

                // Veritabanına sadece Web yolunu kaydediyoruz
                book.ImagePath = $"/images/{fileName}";

                await _context.SaveChangesAsync();

                TempData["Msg"] = "Fotoğraf başarıyla yüklendi.";
            }
            else
            {
                TempData["Error"] = "Geçersiz dosya.";
            }
            _logger.LogInformation("Veritabanına kaydedilen ImagePath: {Path}", book.ImagePath);


            return RedirectToAction("Detail", new { id });
        }
    }
}