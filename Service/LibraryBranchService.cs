using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Services
{
    public class LibraryBranchService
    {
        private readonly AppDbContext _db;
        public LibraryBranchService(AppDbContext db) => _db = db;

        public async Task<List<LibraryBranch>> GetAllBranchesAsync()
        {
            return await _db.LibraryBranches
                            .Include(b => b.Books)
                            .Include(b => b.Customers)
                            .ToListAsync();
        }

        public async Task<LibraryBranch?> GetBranchAsync(int id)
        {
            return await _db.LibraryBranches
                            .Include(b => b.Books)
                            .Include(b => b.Customers)
                            .FirstOrDefaultAsync(b => b.LibraryBranchId == id);
        }

        public async Task AddBranchAsync(LibraryBranch branch)
        {
            _db.LibraryBranches.Add(branch);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateBranchAsync(LibraryBranch branch)
        {
            _db.LibraryBranches.Update(branch);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteBranchAsync(int id)
        {
            var branch = await _db.LibraryBranches.FindAsync(id);
            if (branch != null)
            {
                _db.LibraryBranches.Remove(branch);
                await _db.SaveChangesAsync();
            }
        }
    }
}