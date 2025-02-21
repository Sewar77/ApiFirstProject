using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;
        public AdminController(ModelContext context)
        {
            _context = context;
        }

        public IActionResult JoinTables()
        {
            var customers = _context.Customers.ToList();
            var categories = _context.Categories.ToList();
            var products = _context.Products.ToList();
            var productCustomers = _context.ProductCustomers.ToList();

            var result = from c in customers
                             join pc in productCustomers
                             on c.Id equals pc.CustomerId
                             join p in products
                             on pc.ProductId equals p.ProductId
                             join cat in categories
                             on p.CategoryId equals cat.Id

                             select new JoinTaples
                             {
                                 Product = p,
                                 Customer = c,
                                 Category = cat,
                                 ProductCustomer = pc,
                             };


            return View(result);
        }


        public IActionResult Index()
        {
            ViewData["name"] = HttpContext.Session.GetString("AdminName");
            ViewData["id"] = HttpContext.Session.GetInt32("AdminID");


            ViewBag.CustomerCount = _context.Customers.Count();
            ViewBag.totalSales = _context.Products.Sum(x => x.Sale);
            ViewData["maxPrice"] = _context.Products.Max(x => x.Price);
            ViewData["minPrice"] = _context.Products.Min(x => x.Price);

            
            var customers = _context.Customers.ToList();
            var categories = _context.Categories.ToList(); 
            var products = _context.Products.ToList();

            var finalResult = Tuple.Create<IEnumerable<Product>,IEnumerable<Category>, IEnumerable<Customer>>(products, categories, customers);

            //var tuple1 = Tuple.Create<int, char, string>(12, 'a', "sewar");
            // this is how to create a tuple
            return View(finalResult);
        }

        public IActionResult Search()
        {
            var result = _context.ProductCustomers.Include(x => x.Product).Include(x=> x.Customer).ToList();
         return View(result);
        }

        [HttpPost]
        public IActionResult Search(DateTime? startDate, DateTime? endDate)
        {
            var result = _context.ProductCustomers.Include(x => x.Product).Include(x => x.Customer).ToList();

            if (startDate != null && endDate != null)
            {
                return View(result);
            }
            else if (startDate != null && endDate == null) 
            {
                result = result.Where(x=> x.DateFrom.Value.Date >=  startDate).ToList();    
                return View(result);
            }
            else if (startDate == null && endDate != null)
            {
                result = result.Where(x => x.DateFrom.Value.Date <= endDate).ToList();
                return View(result);
            }

            else
            {
                result = result.Where(x => x.DateFrom.Value.Date <= endDate && x.DateFrom.Value.Date >= startDate).ToList();
                return View(result);
            }


        }

    }
}
