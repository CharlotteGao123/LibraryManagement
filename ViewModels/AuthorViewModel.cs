namespace LibraryManagement.ViewModels
{
    public class AuthorViewModel
    {
        public int AuthorId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Nationality { get; set; }
        public string? Email { get; set; }
        public int BooksCount { get; set; }
        public string? Biography { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}