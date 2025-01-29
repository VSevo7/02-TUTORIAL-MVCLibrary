using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCLibrary.Data;
using MVCLibrary.Models;

namespace MVCLibrary.Controllers
{
    public class BooksController : Controller
    {
        private readonly MVCLibraryContext _context;

        public BooksController(MVCLibraryContext context) => _context = context;

        public async Task<IActionResult> Index() => View(await _context.Book.ToListAsync());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var book = await _context.Book.FirstOrDefaultAsync(m => m.Id == id);
            return book == null ? NotFound() : View(book);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ISBN,Author")] Book book)
        {
            if (!ModelState.IsValid) return View(book);
            _context.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var book = await _context.Book.FindAsync(id);
            return book == null ? NotFound() : View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ISBN,Author")] Book book)
        {
            if (id != book.Id) return NotFound();
            if (!ModelState.IsValid) return View(book);

            try
            {
                _context.Update(book);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Book.Any(e => e.Id == book.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var book = await _context.Book.FirstOrDefaultAsync(m => m.Id == id);
            return book == null ? NotFound() : View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book != null) _context.Book.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
