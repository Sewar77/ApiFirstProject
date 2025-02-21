using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessLife.Models;

namespace FitnessLife.Controllers
{
    public class FeedbacksController : Controller
    {
        private readonly ModelContext _context;

        public FeedbacksController(ModelContext context)
        {
            _context = context;
        }


        // GET: Feedbacks/Index
        public async Task<IActionResult> Index()
        {
            var feedbacks = await _context.Feedbacks.ToListAsync();
            return View(feedbacks);
        }
    


    // GET: Feedbacks/Details/5
    public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Feedbacks == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedbacks
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Feedbackid == id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // GET: Feedbacks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Feedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Feedbacks/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                _context.Add(feedback);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(feedback);
        }


        // GET: Feedbacks/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Feedbacks == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", feedback.Userid);
            return View(feedback);
        }

        // POST: Feedbacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Feedbackid,Userid,Feedbacktext,Submittedat,Approved")] Feedback feedback)
        {
            if (id != feedback.Feedbackid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feedback);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedbackExists(feedback.Feedbackid))
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
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", feedback.Userid);
            return View(feedback);
        }

        // GET: Feedbacks/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Feedbacks == null)
            {
                return NotFound();
            }

            var feedback = await _context.Feedbacks
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Feedbackid == id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // POST: Feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Feedbacks == null)
            {
                return Problem("Entity set 'ModelContext.Feedbacks'  is null.");
            }
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback != null)
            {
                _context.Feedbacks.Remove(feedback);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeedbackExists(decimal id)
        {
          return (_context.Feedbacks?.Any(e => e.Feedbackid == id)).GetValueOrDefault();
        }
    }
}
