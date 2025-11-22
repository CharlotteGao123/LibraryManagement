using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    /// <summary>
    /// Represents a customer/member in the library system.
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Unique identifier for the customer
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Customer's first name (required, max 100 characters)
        /// </summary>
        [Required, StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Customer's last name (required, max 100 characters)
        /// </summary>
        [Required, StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Customer's email address
        /// </summary>
        [EmailAddress]
        public string? Email { get; set; }

        /// <summary>
        /// Customer's phone number
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// Foreign key to LibraryBranch</summary>
        // FK
        [Required]
        public int LibraryBranchId { get; set; }
        public LibraryBranch? LibraryBranch { get; set; }
    }
}