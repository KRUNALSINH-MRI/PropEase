using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PropEase.Data;
using PropEase.Models;
using System.Linq;

namespace PropEase.Controllers
{
    [Authorize(Roles = "Owner")]
    public class OwnerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OwnerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Dashboard()
        {
            var ownerId = _userManager.GetUserId(User);

            ViewBag.TotalProperties = _context.Properties
                .Where(p => p.OwnerId == ownerId).Count();

            ViewBag.TotalMessages = _context.PropertyMessages
                .Where(m => m.OwnerId == ownerId).Count();

            // Recent 5 properties
            ViewBag.RecentMyProperties = _context.Properties
                .Where(p => p.OwnerId == ownerId)
                .OrderByDescending(p => p.PostedOn)
                .Take(5)
                .ToList();

            // Recent 5 messages
            ViewBag.RecentMyMessages = _context.PropertyMessages
                .Where(m => m.OwnerId == ownerId)
                .OrderByDescending(m => m.SentOn)
                .Take(5)
                .ToList();


            return View();
        }

        // AJAX: Get Owner Properties
        public IActionResult GetProperties()
        {
            var ownerId = _userManager.GetUserId(User);

            var properties = _context.Properties
                .Where(p => p.OwnerId == ownerId)
                .Select(p => new {
                    p.Title,
                    CreatedDate = p.PostedOn.ToString("yyyy-MM-dd")
                })
                .ToList();

            return Json(properties);
        }


        // AJAX: Get Messages
        public IActionResult GetMessages()
        {
            var ownerId = _userManager.GetUserId(User);

            var messages = _context.PropertyMessages
                .Where(m => m.OwnerId == ownerId)
                .OrderByDescending(m => m.SentOn)
                .Select(m => new {
                    m.PropertyTitle,
                    m.Message,
                    SentOn = m.SentOn.ToString("yyyy-MM-dd")
                })
                .ToList();

            return Json(messages);
        }
    }
}
