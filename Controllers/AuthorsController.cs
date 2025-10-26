using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.ViewModels;

namespace LibraryManagement.Controllers
{
    [Route("Authors")]
    public class AuthorsController : Controller
    {
        private readonly AppDbContext _db;
        public AuthorsController(AppDbContext db) => _db = db;

        // GET /Authors/List
        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            var authors = await _db.Authors
                .Include(a => a.Books) //book count
                .ToListAsync();

            return View("DetailsList", authors);
        }

        // GET /Authors/Details/5
        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var author = await _db.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.AuthorId == id);

            if (author == null) return NotFound();
            return View(author);
        }

        // GET /Authors/Create
        [HttpGet("Create")]
        public IActionResult Create() => View();

        // POST /Authors/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Author author)
        {
            if (!ModelState.IsValid) return View(author);

            _db.Authors.Add(author);
            await _db.SaveChangesAsync();
            return RedirectToAction("List");
        }

        // GET /Authors/Edit/5
        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var author = await _db.Authors.FindAsync(id);
            if (author == null) return NotFound();
            return View(author);
        }

        // POST /Authors/Edit/5
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Author author)
        {
            if (id != author.AuthorId) return BadRequest();
            if (!ModelState.IsValid) return View(author);

            _db.Update(author);
            await _db.SaveChangesAsync();
            return RedirectToAction("List");
        }

        // GET /Authors/Delete/5
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _db.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.AuthorId == id);

            if (author == null) return NotFound();

            // Don't block it here, show the confirmation page, and do a "soft cascade" deletion when POSTing
            ViewBag.BooksCount = author.Books?.Count ?? 0;
            return View(author); // Views/Authors/Delete.cshtml
        }

        // POST /Authors/Delete/5
        [HttpPost("Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Key: Delete the associated book together, otherwise it will trigger an FK error
            var author = await _db.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.AuthorId == id);

            if (author == null) return NotFound();


            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                if (author.Books != null && author.Books.Any())
                    _db.Books.RemoveRange(author.Books);

                _db.Authors.Remove(author);
                await _db.SaveChangesAsync();

                await tx.CommitAsync();
                return RedirectToAction("List");
            }
            catch (DbUpdateException)
            {
                await tx.RollbackAsync();
                TempData["ErrorMessage"] = "Delete failed due to foreign key constraints.";
                return RedirectToAction("List");
            }
        }
    }
}