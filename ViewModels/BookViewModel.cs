namespace LibraryManagement.ViewModels
{
    public class BookViewModel
    {
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? ISBN { get; set; }
        public int? PublishedYear { get; set; }
        public decimal? Price { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
    }
}