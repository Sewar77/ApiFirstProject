using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;


namespace RestaurantMVC.Controllers
{
    public class RegisterAndLoginController : Controller
    {

        private readonly ModelContext _context;
        //6.IWebHostEnvironment
        private readonly IWebHostEnvironment _webHostEnvironment;
        public RegisterAndLoginController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Fname,Lname,ImagePath,ImageFile")] Customer customer, string username, string password)
        {
            if (ModelState.IsValid)
            {
                
                await _context.SaveChangesAsync();
                UserLogin login = new UserLogin();
                login.UserName = username;
                login.Passwordd = password;
                login.CustomerId = customer.Id;
                login.RoleId = 2;
                _context.Add(login);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View(customer);
        }







        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("UserName,Passwordd")] UserLogin userLogin)
        {

            var auth = _context.UserLogins.Where(x => x.UserName == userLogin.UserName && x.Passwordd == userLogin.Passwordd).SingleOrDefault();
            if (auth != null)
            {
                switch (auth.RoleId)
                {
                    case 1://Admin
                    

                        HttpContext.Session.SetString("AdminName", auth.UserName);
                        HttpContext.Session.SetInt32("AdminId", (int)auth.Id);
                        return RedirectToAction("Index", "Admin");

                    case 2://Customer 
                        HttpContext.Session.SetInt32("CustomerId", (int)auth.Id);
                        return RedirectToAction("Index", "Home");

                    default:
                        ModelState.AddModelError(string.Empty, "Invalid role assigned.");
                        break;
                }
            }
            
            return View();
        }
    }

}
