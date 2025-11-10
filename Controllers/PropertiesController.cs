using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PropEase.Data;
using PropEase.Models;
using System;
using System.Buffers.Text;
using System.Linq;
using System.Threading.Tasks;

namespace PropEase.Controllers
{
    public class PropertiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public PropertiesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {  
            _context = context;
            _userManager = userManager;
        }

        // GET: Properties
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.Properties.Include(p => p.Owner);
        //    return View(await applicationDbContext.ToListAsync());
        //}

        // GET: Properties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var property = await _context.Properties
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (property == null)
                return NotFound();

            return View(property);
        }

        // GET: Properties/Create
        [Authorize(Roles = "Admin,Owner")]
        public IActionResult Create()
        {
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Properties/Create
        [Authorize(Roles = "Admin,Owner")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Property property, IFormFile? ImageFile)
        {
            // Manual validation: Must provide either file or URL
            if ((ImageFile == null || ImageFile.Length == 0) && string.IsNullOrWhiteSpace(property.ImageUrl))
            {
                ModelState.AddModelError("ImageUrl", "Please upload an image or provide an image URL.");
                return View(property);
            }

            if (ModelState.IsValid)
            {
                // If file is uploaded, save it
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/properties");
                    Directory.CreateDirectory(uploadsFolder); // ensures folder exists

                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    property.ImageUrl = "/images/properties/" + fileName; // save relative path
                }

                property.PostedOn = DateTime.Now;
                var userId = _userManager.GetUserId(User);
                property.OwnerId = userId;

                _context.Add(property);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(property);
        }





        // GET: Properties/Edit/5
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var property = await _context.Properties.FindAsync(id);
            if (property == null)
                return NotFound();

            return View(property);
        }

        // POST: Properties/Edit/5
        // POST: Properties/Edit/5
        [Authorize(Roles = "Admin,Owner")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Property property, IFormFile? ImageFile)
        {
            if (id != property.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // ✅ Get the existing property from DB first
                    var existingProperty = await _context.Properties.AsNoTracking()
                        .FirstOrDefaultAsync(p => p.Id == id);

                    if (existingProperty == null)
                        return NotFound();

                    // ✅ Preserve the original OwnerId and PostedOn date
                    property.OwnerId = existingProperty.OwnerId;
                    property.PostedOn = existingProperty.PostedOn;

                    // ✅ Handle image upload (if new one provided)
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/properties");
                        Directory.CreateDirectory(uploadsFolder);

                        var fileName = Path.GetFileName(ImageFile.FileName);
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(stream);
                        }

                        property.ImageUrl = "/images/properties/" + fileName;
                    }
                    else
                    {
                        // ✅ Keep existing image if no new file is uploaded
                        property.ImageUrl = existingProperty.ImageUrl;
                    }

                    _context.Update(property);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Properties.Any(e => e.Id == property.Id))
                        return NotFound();
                    else
                        throw;
                }
            }

            return View(property);
        }



        // GET: Properties/Delete/5
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var property = await _context.Properties
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (property == null)
                return NotFound();

            return View(property);
        }

        // POST: Properties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property != null)
                _context.Properties.Remove(property);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PropertyExists(int id)
        {
            return _context.Properties.Any(e => e.Id == id);
        }

        // PropertiesController.cs
        public async Task<IActionResult> Index(string search, string type, string city, decimal? minPrice, decimal? maxPrice)
        {
            var q = _context.Properties
                .Include(p => p.Owner) // 👈 add this line
                .AsQueryable();


            // 🧠 Filter properties based on logged -in user role
            if (User.IsInRole("Owner"))
            {
                var userId = _userManager.GetUserId(User);
                q = q.Where(p => p.OwnerId == userId);
            }
            else if (User.IsInRole("Admin"))
            {
                // Admin sees everything (no filter)
            }
            else
            {
                // Customer also sees all — but actions are hidden in the view
            }



            if (!string.IsNullOrEmpty(search))
                q = q.Where(p => p.Title.Contains(search) ||
                                 p.Address.Contains(search) ||
                                 p.City.Contains(search) ||
                                 p.State.Contains(search));

            if (!string.IsNullOrEmpty(type))
                q = q.Where(p => p.Type == type);

            if (!string.IsNullOrEmpty(city))
                q = q.Where(p => p.City == city);

            if (minPrice.HasValue)
                q = q.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                q = q.Where(p => p.Price <= maxPrice.Value);

            ViewBag.Types = await _context.Properties
                .Select(p => p.Type)
                .Distinct()
                .ToListAsync();

            ViewBag.Cities = await _context.Properties
                .Select(p => p.City)
                .Distinct()
                .ToListAsync();

            ViewBag.CurrentSearch = search;
            ViewBag.CurrentType = type;
            ViewBag.CurrentCity = city;
            ViewBag.CurrentMinPrice = minPrice;
            ViewBag.CurrentMaxPrice = maxPrice;

            return View(await q.ToListAsync());
        }




    }
}
