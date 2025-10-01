using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Services
{
    public class CustomerService
    {
        private readonly AppDbContext _db;
        public CustomerService(AppDbContext db) => _db = db;

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _db.Customers.Include(c => c.LibraryBranch).ToListAsync();
        }

        public async Task<Customer?> GetCustomerAsync(int id)
        {
            return await _db.Customers.Include(c => c.LibraryBranch)
                                      .FirstOrDefaultAsync(c => c.CustomerId == id);
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _db.Customers.Update(customer);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _db.Customers.FindAsync(id);
            if (customer != null)
            {
                _db.Customers.Remove(customer);
                await _db.SaveChangesAsync();
            }
        }
    }
}