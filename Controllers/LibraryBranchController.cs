using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    [Route("LibraryBranches")]
    public class LibraryBranchesController : Controller
    {
        private readonly AppDbContext _db;
        public LibraryBranchesController(AppDbContext db) => _db = db;

        // GET /LibraryBranches/List
        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            var branches = await _db.LibraryBranches
                .Include(b => b.Books)
                .Include(b => b.Customers)
                .ToListAsync();

            return View("DetailsList", branches);
        }

        // GET /LibraryBranches/Details/5
        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var branch = await _db.LibraryBranches
                .Include(b => b.Books)
                .Include(b => b.Customers)
                .FirstOrDefaultAsync(b => b.LibraryBranchId == id);

            if (branch == null) return NotFound();
            return View(branch);
        }

        // GET /LibraryBranches/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST /LibraryBranches/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LibraryBranch branch)
        {
            if (!ModelState.IsValid)
            {
                return View(branch);
            }

            _db.LibraryBranches.Add(branch);
            await _db.SaveChangesAsync();
            return RedirectToAction("List");
        }

        // GET /LibraryBranches/Edit/5
        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var branch = await _db.LibraryBranches.FindAsync(id);
            if (branch == null) return NotFound();
            return View(branch);
        }

        // POST /LibraryBranches/Edit/5
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LibraryBranch branch)
        {
            if (id != branch.LibraryBranchId) return BadRequest();
            if (!ModelState.IsValid) return View(branch);

            _db.Update(branch);
            await _db.SaveChangesAsync();
            return RedirectToAction("List");
        }

        // GET /LibraryBranches/Delete/5
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var branch = await _db.LibraryBranches
                .Include(b => b.Books)
                .Include(b => b.Customers)
                .FirstOrDefaultAsync(b => b.LibraryBranchId == id);

            if (branch == null) return NotFound();
            return View(branch);
        }

        // POST /LibraryBranches/Delete/5
        [HttpPost("Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var branch = await _db.LibraryBranches
                .Include(b => b.Books)
                .Include(b => b.Customers)
                .FirstOrDefaultAsync(b => b.LibraryBranchId == id);

            if (branch == null) return NotFound();

            if (branch.Books.Any() || branch.Customers.Any())
            {
                // Prevent accidental deletion of branches with foreign key dependencies
                ModelState.AddModelError("", "Cannot delete branch with related books or customers.");
                return View("Delete", branch);
            }

            _db.LibraryBranches.Remove(branch);
            await _db.SaveChangesAsync();
            return RedirectToAction("List");
        }

    }
}