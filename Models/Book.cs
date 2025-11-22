using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    /// <summary>
    /// Represents a book in the library system.
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Unique identifier for the book
        /// </summary>
        public int BookId { get; set; }

        /// <summary>
        /// Title of the book (required, max 300 characters)
        /// </summary>
        [Required, StringLength(300)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// ISBN number of the book (10-13 digits format)
        /// </summary>
        [RegularExpression(@"^[0-9\-]{10,13}$", ErrorMessage = "ISBN must be 10-13 digits.")]
        [StringLength(32)]
        public string? ISBN { get; set; }

        /// <summary>
        /// Year the book was published (between 1500 and 2100)
        /// </summary>
        [Range(1500, 2100, ErrorMessage = "Please enter a valid year between 1500 and 2100.")]
        public int? PublishedYear { get; set; }

        /// <summary>
        /// Price of the book (0.01 to 10000)
        /// </summary>
        [Range(0.01, 10000, ErrorMessage = "Price must be between 0.01 and 10000.")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Price { get; set; }

        /// <summary>
        /// Foreign key to Author
        /// </summary>
        [Required]
        public int AuthorId { get; set; }
        
        /// <summary>
        /// Navigation property to Author
        /// </summary>
        public Author? Author { get; set; }

        /// <summary>
        /// Foreign key to LibraryBranch
        /// </summary>
        [Required]
        public int LibraryBranchId { get; set; }
        public LibraryBranch? LibraryBranch { get; set; }
    }
}