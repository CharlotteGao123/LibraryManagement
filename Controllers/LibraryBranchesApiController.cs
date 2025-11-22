using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LibraryBranchesApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LibraryBranchesApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/LibraryBranchesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibraryBranch>>> GetLibraryBranches()
        {
            if (_context.LibraryBranches == null)
            {
                return NotFound();
            }
            return await _context.LibraryBranches
                .Include(b => b.Books)
                .Include(b => b.Customers)
                .ToListAsync();
        }

        // GET: api/LibraryBranchesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LibraryBranch>> GetLibraryBranch(int id)
        {
            if (_context.LibraryBranches == null)
            {
                return NotFound();
            }
            var libraryBranch = await _context.LibraryBranches
                .Include(b => b.Books)
                .Include(b => b.Customers)
                .FirstOrDefaultAsync(b => b.LibraryBranchId == id);

            if (libraryBranch == null)
            {
                return NotFound();
            }

            return libraryBranch;
        }

        // PUT: api/LibraryBranchesApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLibraryBranch(int id, LibraryBranch libraryBranch)
        {
            if (id != libraryBranch.LibraryBranchId)
            {
                return BadRequest();
            }

            _context.Entry(libraryBranch).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibraryBranchExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/LibraryBranchesApi
        [HttpPost]
        public async Task<ActionResult<LibraryBranch>> PostLibraryBranch(LibraryBranch libraryBranch)
        {
            if (_context.LibraryBranches == null)
            {
                return Problem("Entity set 'AppDbContext.LibraryBranches' is null.");
            }
            _context.LibraryBranches.Add(libraryBranch);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLibraryBranch", new { id = libraryBranch.LibraryBranchId }, libraryBranch);
        }

        // DELETE: api/LibraryBranchesApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibraryBranch(int id)
        {
            if (_context.LibraryBranches == null)
            {
                return NotFound();
            }
            var libraryBranch = await _context.LibraryBranches
                .Include(b => b.Books)
                .Include(b => b.Customers)
                .FirstOrDefaultAsync(b => b.LibraryBranchId == id);
            
            if (libraryBranch == null)
            {
                return NotFound();
            }

            _context.LibraryBranches.Remove(libraryBranch);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LibraryBranchExists(int id)
        {
            return (_context.LibraryBranches?.Any(e => e.LibraryBranchId == id)).GetValueOrDefault();
        }
    }
}
