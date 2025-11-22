using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    /// <summary>
    /// Represents an author in the library system.
    /// </summary>
    public class Author
    {
        /// <summary>
        /// Unique identifier for the author
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Author's full name (required, max 200 characters)
        /// </summary>
        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Author's biography
        /// </summary>
        public string? Biography { get; set; }
        
        /// <summary>
        /// Author's date of birth
        /// </summary>
        public DateTime? BirthDate { get; set; }
        
        /// <summary>
        /// Author's nationality
        /// </summary>
        public string? Nationality { get; set; }

        /// <summary>
        /// Author's email address
        /// </summary>
        [EmailAddress]
        public string? Email { get; set; }

        /// <summary>
        /// Navigation property to all books by this author
        /// </summary>
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}