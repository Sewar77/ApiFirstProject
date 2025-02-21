using FitnessLife.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessLife.Controllers
{
    public class RegisterAndLoginController : Controller
    {
		private readonly ModelContext _context;

		public RegisterAndLoginController(ModelContext context)
		{
			_context = context;
		}
		public IActionResult Index()
        {
            return View();
        }
		public async Task<IActionResult> RegisterAsync(string tab, User model)
		{
			// Pass the "tab" value to the view to determine which tab to show
			ViewData["Tab"] = tab;

			if (ModelState.IsValid)
			{
				// Set the initial status to "Pending"
				model.Status = "Pending";

				// Save the user to the database
				_context.Users.Add(model);
				await _context.SaveChangesAsync();

				// Redirect to the login page or another confirmation page
				return RedirectToAction("Index", "Home");
			}

			// Return the view with model errors if there are any
			return View(model);

		}
	}
}
