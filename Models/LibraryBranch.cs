using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    /// <summary>
    /// Represents a library branch in the library system.
    /// </summary>
    public class LibraryBranch
    {
        /// <summary>
        /// Unique identifier for the library branch
        /// </summary>
        public int LibraryBranchId { get; set; }

        /// <summary>
        /// Name of the library branch (required, max 150 characters)
        /// </summary>
        [Required, StringLength(150)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Physical address of the library branch
        /// </summary>
        public string? Address { get; set; }
        
        /// <summary>
        /// Phone number of the library branch
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// Navigation property to all books in this branch
        /// </summary>
        public ICollection<Book> Books { get; set; } = new List<Book>();
        
        /// <summary>
        /// Navigation property to all customers registered at this branch
        /// </summary>
        public ICollection<Customer> Customers { get; set; } = new List<Customer>();
    }
}
