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
    public class MembersController : Controller
    {
        private readonly LibraryContext _context;

        public MembersController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Members
        public async Task<IActionResult> Index(string? errorMessage = null)
        {
            ViewBag.LoginError = errorMessage;
            return View(await _context.Members.ToListAsync());
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            var currentReservations = await _context.Loans.Include(r => r.LentBook).Where(r => r.LoaneeId == id && !r.IsReturned).ToListAsync();
            Console.WriteLine("Incoming MemberId: " + id);
            var pastReservations = await _context.Loans.Include(r => r.LentBook).Where(r => r.LoaneeId == id && r.IsReturned).ToListAsync();

            ViewBag.CurrentReservations = currentReservations;
            ViewBag.PastReservations = pastReservations;

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Password,PhoneNumber,Email,SignUpDate,Fine")] Member member)
        {
            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Account created successfully! Please log in.";
                return RedirectToAction("Details");
            }
            else
            {
                TempData["ErrorMessage"] = "Unsuccessful . Please try again Later.";
            }
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index", "Members");
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Password,PhoneNumber,Email,SignUpDate,Fine")] Member member, string returnUrl)
        {
            if (id != member.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Details", "Members", new { id = member.Id });
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                _context.Members.Remove(member);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("AdminPanel", "Home");
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.Id == id);
        }

        // GET: Members/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Members/Login
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.Name == username && m.Password == password);

            if (member != null)
            {
                HttpContext.Session.SetInt32("MemberId", member.Id);
                HttpContext.Session.SetString("MemberName", member.Name);

                return RedirectToAction("Details", "Members", new { id = member.Id });
            }
            else
            {
                TempData["ErrorMessage"] = "Username or Password is incorrect.";
            }
                return View("Index");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Headers["Cache-Control"] = "no-store";
            return RedirectToAction("Index", "Members");  
        }

        [HttpPost]
        public async Task<IActionResult> PayFine(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            member.Fine = 0;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Your fine has been paid.";
            return RedirectToAction("Details", new { id = id });
        }

        // GET: Members/CreateAdmin
        [HttpGet]
        public IActionResult CreateAdmin()
        {
            return View();
        }

        // POST: Members/CreateAdmin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdmin(Member member)
        {
            if (ModelState.IsValid)
            {
                member.IsAdmin = true;
                member.MaxReserveNum = 5;
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction("AdminPanel", "Home");
            }
            return View(member);
        }



    }
}
