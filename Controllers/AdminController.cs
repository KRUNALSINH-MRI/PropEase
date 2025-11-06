using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PropEase.Data;
using PropEase.Models;
using System.Linq;

namespace PropEase.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Dashboard()
        {
            ViewBag.TotalUsers = _userManager.Users.Count();
            ViewBag.TotalProperties = _context.Properties.Count();
            ViewBag.TotalOwners = _userManager.GetUsersInRoleAsync("Owner").Result.Count;
            ViewBag.TotalClients = _userManager.GetUsersInRoleAsync("Client").Result.Count;
            ViewBag.TotalMessages = _context.ContactMessages.Count();

            ViewBag.RecentProperties = _context.Properties
                .OrderByDescending(p => p.PostedOn)
                .Take(5)
                .ToList();

            ViewBag.RecentMessages = _context.ContactMessages
                .OrderByDescending(m => m.CreatedAt)
                .Take(5)
                .ToList();

            return View();
        }
    }
}
