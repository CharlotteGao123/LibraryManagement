using System.ComponentModel.DataAnnotations;
using LibraryManagement.Models;
namespace LibraryManagement.ViewModels
{
    public class BookViewModel
    {
        public int BookId { get; set; }

        [Required, StringLength(300)]
        public string Title { get; set; } = string.Empty;

        [RegularExpression(@"^[0-9\-]{10,13}$", ErrorMessage = "ISBN must be 10-13 digits.")]
        [StringLength(32)]
        public string? ISBN { get; set; }

        [Range(1500, 2100, ErrorMessage = "Please enter a valid year between 1500 and 2100.")]
        public int? PublishedYear { get; set; }

        [Range(0.01, 10000, ErrorMessage = "Price must be between 0.01 and 10000.")]
        public decimal? Price { get; set; }

        [Required]
        public int AuthorId { get; set; }
        public Author? Author { get; set; }

        [Required]
        public int LibraryBranchId { get; set; }
        public LibraryBranch? LibraryBranch { get; set; }
    }
}