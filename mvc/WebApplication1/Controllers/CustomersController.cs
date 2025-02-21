using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace Restaurant.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CustomersController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return _context.Customers != null
                ? View(await _context.Customers.ToListAsync())
                : Problem("Entity set 'ModelContext.Customers' is null.");
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fname,Lname,ImageFile")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + customer.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await customer.ImageFile.CopyToAsync(fileStream);
                }

                customer.ImagePath = fileName;

                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }
        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            ViewBag.CurrentImagePath = customer.ImagePath;
            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Fname,Lname,ImageFile")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingCustomer = await _context.Customers.FindAsync(id);
                    if (existingCustomer == null)
                    {
                        return NotFound();
                    }

                    string wwwRootPath = _webHostEnvironment.WebRootPath;

                    // Handling image file upload if a new image is provided
                    if (customer.ImageFile != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + "_" + customer.ImageFile.FileName;
                        string newPath = Path.Combine(wwwRootPath, "Images", fileName);

                        // Save the new image
                        using (var fileStream = new FileStream(newPath, FileMode.Create))
                        {
                            await customer.ImageFile.CopyToAsync(fileStream);
                        }

                        // Remove the old image
                        if (!string.IsNullOrEmpty(existingCustomer.ImagePath))
                        {
                            string oldImagePath = Path.Combine(wwwRootPath, "Images", existingCustomer.ImagePath);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        existingCustomer.ImagePath = fileName;
                    }

                    // Update other properties
                    existingCustomer.Fname = customer.Fname;
                    existingCustomer.Lname = customer.Lname;

                    // Save changes to the database
                    _context.Update(existingCustomer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // If ModelState is invalid, return to the view with current data
            ViewBag.CurrentImagePath = customer.ImagePath;
            return View(customer);
        }

        private bool CustomerExists(decimal id)
        {
            throw new NotImplementedException();
        }


        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                // Delete the image if it exists
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (!string.IsNullOrEmpty(customer.ImagePath))
                {
                    string imagePath = Path.Combine(wwwRootPath, "Images", customer.ImagePath);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                // Remove dependent ProductCustomer entries
                var productCustomerEntries = await _context.ProductCustomers
                    .Where(pc => pc.CustomerId == id)
                    .ToListAsync();

                if (productCustomerEntries.Any())
                {
                    _context.ProductCustomers.RemoveRange(productCustomerEntries);
                    await _context.SaveChangesAsync(); // Save after removing dependent records
                }

                // Now delete the customer
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}