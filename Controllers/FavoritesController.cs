using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropEase.Data;
using PropEase.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PropEase.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavoritesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Add or remove a property from favorites
        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int propertyId)
        {
            var user = await _userManager.GetUserAsync(User);
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.PropertyId == propertyId && f.UserId == user.Id);

            if (favorite == null)
            {
                _context.Favorites.Add(new Favorite
                {
                    PropertyId = propertyId,
                    UserId = user.Id
                });
            }
            else
            {
                _context.Favorites.Remove(favorite);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Properties");
        }

        // Display user's favorite properties
        public async Task<IActionResult> MyFavorites()
        {
            var user = await _userManager.GetUserAsync(User);
            var favorites = await _context.Favorites
                .Include(f => f.Property)
                .Where(f => f.UserId == user.Id)
                .ToListAsync();

            return View(favorites);
        }
    }
}
