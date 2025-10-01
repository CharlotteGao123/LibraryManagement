using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class LibraryBranch
    {
        public int LibraryBranchId { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; } = string.Empty;

        public string? Address { get; set; }
        public string? Phone { get; set; }

        // Nav
        public ICollection<Book> Books { get; set; } = new List<Book>();
        public ICollection<Customer> Customers { get; set; } = new List<Customer>();
    }
}