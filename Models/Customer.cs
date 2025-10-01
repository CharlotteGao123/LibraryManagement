using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        public string? Phone { get; set; }

        // FK
        [Required]
        public int LibraryBranchId { get; set; }
        public LibraryBranch? LibraryBranch { get; set; }
    }
}