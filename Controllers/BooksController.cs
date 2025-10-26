using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.ViewModels;

namespace LibraryManagement.Controllers
{
    [Route("Books")]
    public class BooksController : Controller
    {
        private readonly AppDbContext _db;
        public BooksController(AppDbContext db) => _db = db;

        // Redirect /Book/Details → /Books/List
        [HttpGet("/Book/Details")]
        public IActionResult DetailsRedirect()
        {
            return RedirectToAction("List");
        }

        // GET /Books/List
        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            var books = await _db.Books
                .Include(b => b.Author)
                .Include(b => b.LibraryBranch)
                .ToListAsync();

            return View("DetailsList", books);
        }

        // GET /Books/Details/5
        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var book = await _db.Books
                .Include(b => b.Author)
                .Include(b => b.LibraryBranch)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null) return NotFound();
            return View(book);
        }

        // GET /Books/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST /Books/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book, string authorName, string branchName)
        {
            if (!ModelState.IsValid)
            {
                return View(book);
            }

            // Find or create Author
            var author = await _db.Authors.FirstOrDefaultAsync(a => a.Name == authorName);
            if (author == null)
            {
                author = new Author { Name = authorName };
                _db.Authors.Add(author);
                await _db.SaveChangesAsync();
            }

            // Find or create Branch
            var branch = await _db.LibraryBranches.FirstOrDefaultAsync(b => b.Name == branchName);
            if (branch == null)
            {
                branch = new LibraryBranch { Name = branchName };
                _db.LibraryBranches.Add(branch);
                await _db.SaveChangesAsync();
            }

            // Assign FKs
            book.AuthorId = author.AuthorId;
            book.LibraryBranchId = branch.LibraryBranchId;

            _db.Books.Add(book);
            await _db.SaveChangesAsync();

            return RedirectToAction("List");
        }

        // GET /Books/Edit/5
        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _db.Books
                .Include(b => b.Author)
                .Include(b => b.LibraryBranch)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null) return NotFound();

            // Pass Author/Branch names for form
            ViewBag.AuthorName = book.Author?.Name;
            ViewBag.BranchName = book.LibraryBranch?.Name;

            return View(book);
        }

        // POST /Books/Edit/5
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book, string authorName, string branchName)
        {
            if (id != book.BookId) return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(book);
            }

            var existingBook = await _db.Books.FindAsync(id);
            if (existingBook == null) return NotFound();

            // Find or create Author
            var author = await _db.Authors.FirstOrDefaultAsync(a => a.Name == authorName);
            if (author == null)
            {
                author = new Author { Name = authorName };
                _db.Authors.Add(author);
                await _db.SaveChangesAsync();
            }

            // Find or create Branch
            var branch = await _db.LibraryBranches.FirstOrDefaultAsync(b => b.Name == branchName);
            if (branch == null)
            {
                branch = new LibraryBranch { Name = branchName };
                _db.LibraryBranches.Add(branch);
                await _db.SaveChangesAsync();
            }

            // Update fields
            existingBook.Title = book.Title;
            existingBook.ISBN = book.ISBN;
            existingBook.PublishedYear = book.PublishedYear;
            existingBook.Price = book.Price;
            existingBook.AuthorId = author.AuthorId;
            existingBook.LibraryBranchId = branch.LibraryBranchId;

            _db.Update(existingBook);
            await _db.SaveChangesAsync();

            return RedirectToAction("List");
        }

        // GET /Books/Delete/5
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _db.Books
                .Include(b => b.Author)
                .Include(b => b.LibraryBranch)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null) return NotFound();
            return View(book); // return Delete.cshtml
        }

        // POST /Books/Delete/5
        [HttpPost("Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _db.Books.FindAsync(id);
            if (book == null) return NotFound();

            _db.Books.Remove(book);
            await _db.SaveChangesAsync();

            return RedirectToAction("List");
        }
    }
}