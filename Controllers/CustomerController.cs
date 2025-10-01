using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    [Route("Customers")]
    public class CustomersController : Controller
    {
        private readonly AppDbContext _db;
        public CustomersController(AppDbContext db) => _db = db;

        // GET /Customers/List
        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            var customers = await _db.Customers
                .Include(c => c.LibraryBranch)
                .ToListAsync();
            return View("DetailsList", customers);
        }

        // GET /Customers/Details/5
        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var customer = await _db.Customers
                .Include(c => c.LibraryBranch)
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            if (customer == null) return NotFound();
            return View(customer);
        }

        // GET /Customers/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewBag.Branches = _db.LibraryBranches.ToList();
            return View();
        }

        // POST /Customers/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Branches = _db.LibraryBranches.ToList();
                return View(customer);
            }

            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();
            return RedirectToAction("List");
        }

        // GET /Customers/Edit/5
        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _db.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            ViewBag.Branches = _db.LibraryBranches.ToList();
            return View(customer);
        }

        // POST /Customers/Edit/5
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customer customer)
        {
            if (id != customer.CustomerId) return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Branches = _db.LibraryBranches.ToList();
                return View(customer);
            }

            _db.Update(customer);
            await _db.SaveChangesAsync();
            return RedirectToAction("List");
        }

        // GET /Customers/Delete/5
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _db.Customers
                .Include(c => c.LibraryBranch)
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            if (customer == null) return NotFound();
            return View(customer);
        }

        // POST /Customers/Delete/5
        [HttpPost("Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _db.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            _db.Customers.Remove(customer);
            await _db.SaveChangesAsync();
            return RedirectToAction("List");
        }
    }
}