namespace LibraryManagement.ViewModels
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = string.Empty; // e.g. FirstName + " " + LastName
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string BranchName { get; set; } = string.Empty;
    }
}