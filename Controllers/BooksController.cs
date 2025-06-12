using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryManagement2.Data;
using LibraryManagement2.Models;

namespace LibraryManagement2.Controllers
{
    public class BooksController : Controller
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
    string sortOrder,
    string currentFilter,
    string searchString,
    string isbnSearch,
    string genreFilter,
    bool? onlyAvailable,
    int? pageNumber)
        {
            int pageSize = 5;
            int page = pageNumber ?? 1;

            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["AuthorSortParm"] = sortOrder == "Author" ? "author_desc" : "Author";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null || isbnSearch != null || genreFilter != null || onlyAvailable != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentGenre"] = genreFilter;
            ViewData["CurrentISBN"] = isbnSearch;
            ViewData["OnlyAvailable"] = onlyAvailable ?? false;

            var books = _context.Books.AsQueryable();

            // filters
            if (!String.IsNullOrEmpty(searchString))
            {
                var lowerSearch = searchString.ToLower();
                books = books.Where(b =>
                    b.Title.ToLower().Contains(lowerSearch) ||
                    b.Author.ToLower().Contains(lowerSearch));
            }

            if (!String.IsNullOrEmpty(isbnSearch))
            {
                books = books.Where(b => b.ISBN.Contains(isbnSearch));
            }

            if (!String.IsNullOrEmpty(genreFilter))
            {
                books = books.Where(b => b.Genre == genreFilter);
            }

            if (onlyAvailable == true)
            {
                books = books.Where(b => b.AvailableCopies > 0);
            }

            // Sort
            switch (sortOrder)
            {
                case "title_desc":
                    books = books.OrderByDescending(b => b.Title);
                    break;
                case "Author":
                    books = books.OrderBy(b => b.Author);
                    break;
                case "author_desc":
                    books = books.OrderByDescending(b => b.Author);
                    break;
                case "Date":
                    books = books.OrderBy(b => b.PublishedDate);
                    break;
                case "date_desc":
                    books = books.OrderByDescending(b => b.PublishedDate);
                    break;
                default:
                    books = books.OrderBy(b => b.Title);
                    break;
            }

            var totalCount = await books.CountAsync();
            var items = await books.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // Pagination Config
            ViewData["PageNumber"] = page;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalCount / pageSize);

            // Getting Genres
            ViewBag.Genres = await _context.Books
                .Select(b => b.Genre)
                .Distinct()
                .ToListAsync();

            return View(items);
        }






        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.Include(b => b.Reviews).FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,ISBN,PublishedDate,AvailableCopies,Genre")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,ISBN,PublishedDate,AvailableCopies,Genre")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reserve(int bookId)
        {
            var memberId = HttpContext.Session.GetInt32("MemberId");
            var member = await _context.Members.FirstOrDefaultAsync(m => m.Id == memberId);
            if (memberId == null)
            {
                TempData["Error"] = "You must be logged in to reserve a book.";
                return RedirectToAction("Index", "Members");
            }

            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                TempData["Error"] = "Book not found.";
                return RedirectToAction(nameof(Index));
            }

            if (book.AvailableCopies <= 0)
            {
                TempData["Error"] = "No available copies.";
                return RedirectToAction(nameof(Index));
            }

            if (member.MaxReserveNum == 0)
            {
                TempData["Error"] = "You Reached your max capacity of reserves.";
                return RedirectToAction(nameof(Index));
            }

            if (member.Fine >0)
            {
                TempData["Error"] = "Please Pay Your fine first";
                return RedirectToAction(nameof(Index));
            }

            var reservation = new Loan
            {
                BookId = book.Id,
                LoaneeId = memberId.Value,
                LoanDate = DateTime.Now
            };

            book.AvailableCopies--;
            member.MaxReserveNum--;

            _context.Loans.Add(reservation);
            _context.Books.Update(book);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Book reserved successfully!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ReservationHistory(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
                return NotFound();

            var reservations = await _context.Loans
                .Include(l => l.Loanee)
                .Where(l => l.BookId == id)
                .OrderByDescending(l => l.LoanDate)
                .ToListAsync();

            ViewBag.BookTitle = book.Title;
            return View(reservations);
        }



    }
}
