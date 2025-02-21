using FitnessLife.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FitnessLife.Controllers
{
	public class AdminController : Controller
	{
		private readonly ModelContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public AdminController(ModelContext context, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult Index()
		{
			ViewData["name"] = HttpContext.Session.GetString("AdminName");
			ViewData["Email"] = HttpContext.Session.GetString("AdminEmail");

			ViewBag.totalUsers = _context.Users.Count();
			ViewBag.totalRevenue = _context.Subscriptions.Sum(x => x.Amount);

			var activeSubscriptions = _context.Subscriptions
				.Where(s => DateTime.Now >= s.Startdate
							&& DateTime.Now <= s.Enddate
							&& s.Paymentstatus == "Paid")
				.Count();
			ViewBag.ActiveSubscriptions = activeSubscriptions;

			var activeMembers = _context.Subscriptions
				.Where(s => s.Enddate > DateTime.Now)
				.Select(s => s.Userid)
				.Distinct()
				.Count();
			ViewBag.ActiveMembers = activeMembers;

			var inactiveMembers = _context.Users
				.Where(u => !_context.Subscriptions
					.Any(s => s.Userid == u.Userid && s.Enddate > DateTime.Now))
				.Count();
			ViewBag.InactiveMembers = inactiveMembers;

			// For new members this year
			var startOfYear = new DateTime(DateTime.Now.Year, 1, 1);
			var newMembersThisYear = _context.Users
				.Where(u => u.Createdat.HasValue && u.Createdat >= startOfYear) // Handle NULL Createdat
				.Count();
			ViewBag.NewMembersThisYear = newMembersThisYear;

			// For new members this month
			var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
			var newMembersThisMonth = _context.Users
				.Where(u => u.Createdat.HasValue && u.Createdat >= startOfMonth) // Handle NULL Createdat
				.Count();
			ViewBag.NewMembersThisMonth = newMembersThisMonth;

			// Get all users as a list
			var data = _context.Users.ToList(); // Fetch actual User objects

			if (data == null)
			{
				return View(new List<User>());
			}

			return View(data); // Pass the list of User objects to the view
		}


		public IActionResult AdminProfile()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AdminProfile(User user)
		{
			if (ModelState.IsValid)
			{
				string wwwwrootPath = _webHostEnvironment.WebRootPath;
				string FileName = Guid.NewGuid().ToString() + "_" + user.ImageFile.FileName;

				string path = Path.Combine(wwwwrootPath + "/Images/", FileName);

				using (var fileStream = new FileStream(path, FileMode.Create))
				{
					await user.ImageFile.CopyToAsync(fileStream);
				}

				user.ImagePath = FileName;

				_context.Add(user);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(user);
		}
	}
}
