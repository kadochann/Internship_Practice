namespace BookStoresWebAPI.ViewModel
{
    public class BookDeleteViewModel
    {
        public bool IsDeleted { get; set; } // Property to indicate if the book is deleted
        public int Id { get; set; } // Property to hold the unique identifier of the book
        public string Title { get; set; } = ""; // Property to hold the title of the book, initialized to an empty string
        public string Type { get; set; } = "";
        public List<string> AuthorName{ get; set; } = new(); // Property to hold the author's last name, initialized to an empty string
    }
}
