using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ModelContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ModelContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult GetProductbyCatigoryId(int id)
        {
            var product = _context.Products.Where(x => x.CategoryId== id).ToList();
            return View(product);
        }


        public IActionResult Index()
        {
            var carigories = _context.Categories.ToList();
            return View(carigories);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Aboutus()
        {
            return View();
        }

        public IActionResult Menu()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
