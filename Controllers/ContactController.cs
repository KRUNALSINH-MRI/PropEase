using Microsoft.AspNetCore.Mvc;
using PropEase.Data;
using PropEase.Models;

namespace PropEase.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Contact
        public IActionResult Index()
        {
            return View();
        }

        // POST: Contact
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ContactMessage contactMessage)
        {
            // 👇 Add this code at the top of the action to check for validation errors
            if (!ModelState.IsValid)
            {
                foreach (var err in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(err.ErrorMessage); // This will print the validation error in console
                }

                ViewBag.Error = "Please fill in all required fields.";
                return View();
            }

            // ✅ If valid, then save to DB
            _context.ContactMessages.Add(contactMessage);
            _context.SaveChanges();

            ViewBag.Success = "Your message has been sent successfully!";
            ModelState.Clear();

            return View();
        }

    }
}

