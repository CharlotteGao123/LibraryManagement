namespace LibraryManagement.ViewModels
{
    public class LibraryBranchViewModel
    {
        public int LibraryBranchId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public int BooksCount { get; set; }
        public int CustomersCount { get; set; }
    }
}