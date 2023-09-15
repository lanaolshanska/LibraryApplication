using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers
{
	using Data;

	public class CategoryController : Controller
	{
		private readonly ApplicationDbContext _db;

		public CategoryController(ApplicationDbContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			var categories = _db.Categories.ToList();
			return View(categories);
		}
	}
}
