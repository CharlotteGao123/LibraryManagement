using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Services
{
    public class BookService
    {
        private readonly AppDbContext _db;
        public BookService(AppDbContext db) => _db = db;

        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _db.Books
                .Include(b => b.Author)
                .Include(b => b.LibraryBranch)
                .ToListAsync();
        }

        public async Task<Book?> GetBookAsync(int id)
        {
            return await _db.Books
                .Include(b => b.Author)
                .Include(b => b.LibraryBranch)
                .FirstOrDefaultAsync(b => b.BookId == id);
        }

        public async Task AddBookAsync(Book book)
        {
            _db.Books.Add(book);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            _db.Books.Update(book);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _db.Books.FindAsync(id);
            if (book != null)
            {
                _db.Books.Remove(book);
                await _db.SaveChangesAsync();
            }
        }
    }
}