namespace BookStoresWebAPI.ViewModels
{
    public class BookCreateViewModel
    {
        public decimal? Price { get; set; } // Property to hold the price of the book
        public string Title { get; set; }= ""; // Property to hold the title of the book, initialized to an empty string
        public string Type { get; set; } = ""; 
        public string AuthorFirstName { get; set; } = ""; // Property to hold the author's name, initialized to an empty string
        public string AuthorLastName { get; set; } = ""; // Property to hold the author's last name, initialized to an empty string
    }
}
