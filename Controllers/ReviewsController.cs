using LibraryManagement2.Data;
using LibraryManagement2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LibraryManagement2.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly LibraryContext _context;

        public ReviewsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            var libraryContext = _context.Reviews.Include(r => r.Book);
            return View(await libraryContext.ToListAsync());
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create
        public IActionResult Create(int bookId)
        {
            var memberId = HttpContext.Session.GetInt32("MemberId");
            if (memberId == null)
            {
                return RedirectToAction("Login", "Members");
            }

            var review = new Review
            {
                BookId = bookId,
                MemberId = memberId.Value,
                Date = DateTime.Now
            };

            return View(review);
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Rating,Comment,BookId,MemberId")] Review review)
        {
            var memberId = HttpContext.Session.GetInt32("MemberId");
            if (memberId == null)
            {
                return RedirectToAction("Login", "Members");
            }

            review.MemberId = memberId.Value;
            review.Date = DateTime.Now;

            bool alreadyReviewed = await _context.Reviews.AnyAsync(r => r.BookId == review.BookId && r.MemberId == review.MemberId);

            if (alreadyReviewed)
            {
                ModelState.AddModelError("", "You have already submitted a review for this book.");
                return View(review);
            }

            if (ModelState.IsValid)
            {
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Review submitted!";
                return RedirectToAction("Details", "Members", new { id = review.MemberId });
            }

            return View(review);
        }


        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Author", review.BookId);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Rating,Comment,Date,BookId")] Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.Id))
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
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Author", review.BookId);
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("AdminLogin", "Members");
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return RedirectToAction("BookReviews", "Books", new { id = review.BookId });
        }



        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }

        public async Task<IActionResult> BookReviews(int bookId, string sortOrder = "date_desc")
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["RatingSortParm"] = String.IsNullOrEmpty(sortOrder) ? "rating_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "date_asc" ? "date_desc" : "date_asc";

            var reviews = _context.Reviews
                .Include(r => r.Member)  
                .Where(r => r.BookId == bookId);

            reviews = sortOrder switch
            {
                "rating_desc" => reviews.OrderByDescending(r => r.Rating),
                "date_asc" => reviews.OrderBy(r => r.Date),
                "date_desc" => reviews.OrderByDescending(r => r.Date),
                _ => reviews.OrderBy(r => r.Rating),
            };

            var model = await reviews.ToListAsync();

            ViewData["BookId"] = bookId;
            var book = await _context.Books.FindAsync(bookId);
            ViewData["BookTitle"] = book?.Title ?? "Book";

            return View(model);
        }

    }
}
