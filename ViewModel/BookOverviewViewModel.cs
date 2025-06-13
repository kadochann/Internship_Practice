namespace BookStoresWebAPI.ViewModels
{
    public class BookOverviewViewModel
    {
        public IEnumerable<BookStoresWebAPI.Models.Book> Books { get; set; } //IEnumerable Book instead of List<Book> for flexibility
        public IEnumerable<BookStoresWebAPI.Models.Author> Authors { get; set; }
    }
}