using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace Restaurant.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ModelContext _context;
        //6.IWebHostEnvironment
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CategoriesController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return _context.Categories != null ?
                        View(await _context.Categories.ToListAsync()) :
                        Problem("Entity set 'ModelContext.Categories'  is null.");
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryName,ImageFile")] Category category)
        {
            if (ModelState.IsValid)
            {
                //7.preparing image path before creating image

                //getting the wwwroot path
                string wwwwrootPath = _webHostEnvironment.WebRootPath;

                //encrypt for the image name ex: f2hgrttrs_Pizza,png
                string FileName = Guid.NewGuid().ToString() + "_" + category.ImageFile.FileName;

                string path = Path.Combine(wwwwrootPath + "/Images/", FileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await category.ImageFile.CopyToAsync(fileStream);
                }

                category.ImagePath = FileName;

                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // : Categories/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewBag.CurrentImagePath = category.ImagePath;

            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,CategoryName,ImagePath,ImageFile")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (category.ImageFile != null)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;
                        string FileName = Guid.NewGuid().ToString() + "_" + category.ImageFile.FileName;

                        string path = Path.Combine(wwwRootPath + "/Images/", FileName);


                        // Save new image
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await category.ImageFile.CopyToAsync(fileStream);
                        }

                        // Remove old image if it exists
                        if (!string.IsNullOrEmpty(category.ImagePath))
                        {
                            string oldImagePath = Path.Combine(wwwRootPath + "/Images/", FileName);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Update the image path
                        category.ImagePath = FileName;
                    }

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Reassign the current image path if the update fails
            ViewBag.CurrentImagePath = category.ImagePath;
            return View(category);
        }

        private bool CategoryExists(decimal id)
        {
            throw new NotImplementedException();
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            // Get the customer
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            // Delete related child records
            var relatedRecords = _context.Products.Where(o => o.Id == id);
            _context.Products.RemoveRange(relatedRecords);

            // Delete the customer
            _context.Customers.Remove(customer);

            // Save changes
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
