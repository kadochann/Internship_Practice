namespace BookStoresWebAPI.ViewModels
{
    public class BookWithAuthorVM
    {
        public int BookId { get; set; }= 0;
        public string Title { get; set; } = "";
        public string Type { get; set; } = "";
        public decimal? Price { get; set; }
        public List<string> AuthorNames { get; set; } = new();
    }
}
