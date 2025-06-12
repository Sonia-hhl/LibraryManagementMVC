using LibraryManagement2.Data;
using LibraryManagement2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LibraryManagement2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LibraryContext _context;

        public HomeController(ILogger<HomeController> logger, LibraryContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }

        // POST: Home/AdminLogin
        [HttpPost]
        public async Task<IActionResult> AdminLogin(string username, string password)
        {
            var admin = await _context.Members.FirstOrDefaultAsync(m => m.Name == username && m.Password == password && m.IsAdmin);
            if (admin != null)
            {
                HttpContext.Session.SetInt32("MemberId", admin.Id);
                HttpContext.Session.SetString("MemberName", admin.Name);
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToAction("AdminPanel");
            }
            else
            {
                TempData["ErrorMessage"] = "Username or Password is incorrect.";
            }

            ViewData["Error"] = "Invalid username, password, or not an admin account.";
            return View();
        }
          
            public IActionResult Logout()
        {
            HttpContext.Session.Remove("IsAdmin"); 
            return RedirectToAction("AdminLogin");
        }

        public IActionResult AdminPanel(string memberSearch, string bookSearch)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                Console.WriteLine("Not admin");
                return RedirectToAction("AdminLogin");
            }

            var members = string.IsNullOrEmpty(memberSearch)
            ? _context.Members.ToList()
            : _context.Members
                      .Where(m => m.Name.Contains(memberSearch) || m.Email.Contains(memberSearch))
                      .ToList();

            var books = string.IsNullOrEmpty(bookSearch)
                ? _context.Books.ToList()
                : _context.Books
                          .Where(b => b.Title.Contains(bookSearch) || b.Author.Contains(bookSearch))
                          .ToList();

            ViewBag.MemberSearch = memberSearch;
            ViewBag.BookSearch = bookSearch;
            ViewBag.Members = members;
            ViewBag.Books = books;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
