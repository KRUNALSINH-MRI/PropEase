using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropEase.Models;
using PropEase.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PropEase.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        //public IActionResult HomePage()
        //{
        //    return View();
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Index(string city, string type, string search, decimal? minPrice, decimal? maxPrice)
        {
            // ✅ Start with all properties
            var properties = from p in _context.Properties
                             select p;

            // ✅ Apply filters only if they have values
            if (!string.IsNullOrEmpty(city))
            {
                properties = properties.Where(p => p.City == city);
            }

            if (!string.IsNullOrEmpty(type))
            {
                properties = properties.Where(p => p.Type == type);
            }

            if (!string.IsNullOrEmpty(search))
            {
                properties = properties.Where(p =>
                    p.Title.Contains(search) ||
                    p.Address.Contains(search) ||
                    p.City.Contains(search));
            }

            if (minPrice.HasValue)
            {
                properties = properties.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                properties = properties.Where(p => p.Price <= maxPrice.Value);
            }

            // ✅ Populate dropdown data
            ViewBag.Cities = await _context.Properties
                .Select(p => p.City)
                .Distinct()
                .ToListAsync();

            ViewBag.Types = await _context.Properties
                .Select(p => p.Type)
                .Distinct()
                .ToListAsync();

            // ✅ Keep track of current filters for pre-filling the form
            ViewBag.CurrentCity = city;
            ViewBag.CurrentType = type;
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentMinPrice = minPrice;
            ViewBag.CurrentMaxPrice = maxPrice;

            // ✅ Return the filtered list
            return View(await properties.ToListAsync());
        }

    }
}
