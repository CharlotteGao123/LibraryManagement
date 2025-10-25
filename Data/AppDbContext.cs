using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagement.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Book> Books => Set<Book>();
        public DbSet<Author> Authors => Set<Author>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<LibraryBranch> LibraryBranches => Set<LibraryBranch>();

        // Configure the entity: limit field length + unique constraint
        // Another way to figer out this problem is using database tools like Dbeaver
        // How to connect the Dbeaver just create the table in the Debeaver,
        //and then use the path link let project to connect the project
        //You will finally see your data store in this database.
        //But remember that there has limit space for the free Debever, if you don't
        //want to pay extra, please save your space or you can save data in your cloud.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Author>(e =>
            {
                e.Property(p => p.Name).HasMaxLength(200).IsRequired();
                e.HasIndex(p => p.Email).IsUnique();
            });

            modelBuilder.Entity<LibraryBranch>(e =>
            {
                e.Property(p => p.Name).HasMaxLength(150).IsRequired();
                e.HasIndex(p => p.Name).IsUnique();
            });

            modelBuilder.Entity<Book>(e =>
            {
                e.Property(p => p.Title).HasMaxLength(300).IsRequired();
                e.Property(p => p.ISBN).HasMaxLength(32);
                e.HasIndex(p => p.ISBN).IsUnique(false);
                e.HasOne(b => b.Author)
                 .WithMany(a => a.Books)
                 .HasForeignKey(b => b.AuthorId)
                 .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(b => b.LibraryBranch)
                 .WithMany(lb => lb.Books)
                 .HasForeignKey(b => b.LibraryBranchId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Customer>(e =>
            {
                e.Property(p => p.FirstName).HasMaxLength(100).IsRequired();
                e.Property(p => p.LastName).HasMaxLength(100).IsRequired();
                e.HasIndex(p => p.Email).IsUnique();
                e.HasOne(c => c.LibraryBranch)
                 .WithMany(lb => lb.Customers)
                 .HasForeignKey(c => c.LibraryBranchId)
                 .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}