using System.ComponentModel.DataAnnotations;
using LibraryManagement.Models;
using System.Collections.Generic;
namespace LibraryManagement.ViewModels
{
    public class AuthorViewModel
    {
        public int AuthorId { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Biography { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Nationality { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        // Nav
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}