using System.ComponentModel.DataAnnotations;

namespace BookStoresWebAPI.ViewModels
{
    public class BookCreateViewModel
    {
        public decimal? Price { get; set; } // Property to hold the price of the book
        public int Id { get; set; }
        public string Title { get; set; }= ""; // Property to hold the title of the book, initialized to an empty string
        public string Type { get; set; } = "";
        public string AuthorFirstName { get; set; } = ""; 
        public string AuthorLastName { get; set; } = "";
        public string? ImagePath { get; set; } = "";

        public string? Notes { get; set; } //description of the book to fill by the user
    }
}
