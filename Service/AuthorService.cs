using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Services
{
    // Service layer for managing Author entities.
    // Provides CRUD operations and ensures database access 
    //is encapsulated outside of controllers.
    public class AuthorService
    {
        private readonly AppDbContext _db;

        //Constructor: injects AppDbContext through dependency injection.
        public AuthorService(AppDbContext db) => _db = db;

        //Retrieves all authors, including their related books.
        public async Task<List<Author>> GetAllAuthorsAsync()
        {
            return await _db.Authors.Include(a => a.Books).ToListAsync();
        }

        // Retrieves a single author by ID, including their related books.
        // Returns null if not found.
        public async Task<Author?> GetAuthorAsync(int id)
        {
            return await _db.Authors.Include(a => a.Books) // eager loading for related books
                                    .FirstOrDefaultAsync(a => a.AuthorId == id);
        }

        public async Task AddAuthorAsync(Author author)
        {
            _db.Authors.Add(author);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAuthorAsync(Author author)
        {
            _db.Authors.Update(author);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAuthorAsync(int id)
        {
            var author = await _db.Authors.FindAsync(id);
            if (author != null)
            {
                _db.Authors.Remove(author);
                await _db.SaveChangesAsync();
            }
        }
    }
}