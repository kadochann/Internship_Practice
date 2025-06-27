using BookStoresWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using BookStoresWebAPI.Data;
using System;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase 
{
    private readonly BookStoresDbContext _context;

    public BooksController(BookStoresDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetBooks()
    {
        var books = _context.Books.ToList();
        return Ok(books); // HTTP 200 + JSON
    }

    [HttpPost]
    public IActionResult AddBook([FromBody] Book book)
    {
        _context.Books.Add(book);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetBooks), new { id = book.BookId }, book);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBook(int id)
    {
        var book = _context.Books.Find(id);
        if (book == null)
            return NotFound();

        _context.Books.Remove(book);
        _context.SaveChanges();
        return NoContent(); // HTTP 204
    }
}
