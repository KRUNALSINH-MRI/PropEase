using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PropEase.Data;
using PropEase.Models;
using Microsoft.AspNetCore.Authorization;


namespace PropEase.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessagesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Messages/ContactOwner/5
        public async Task<IActionResult> ContactOwner(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property == null)
                return NotFound();

            var model = new PropertyMessage
            {
                PropertyId = id,
                PropertyTitle = property.Title,
                OwnerId = property.OwnerId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactOwner(PropertyMessage model)
        {
            if (ModelState.IsValid)
            {
                var property = await _context.Properties.FindAsync(model.PropertyId);
                if (property != null)
                {
                    model.PropertyTitle = property.Title; // ✅ Reassign server-side
                }
                // Ensure EF doesn’t think we’re updating an existing record
                model.Id = 0;

                model.SenderId = _userManager.GetUserId(User);
                model.SentOn = DateTime.Now;

                _context.PropertyMessages.Add(model);
                await _context.SaveChangesAsync();

                ViewBag.Success = "Your message has been sent to the owner!";
                return View(model);
            }

            return View(model);
        }


    }
}
