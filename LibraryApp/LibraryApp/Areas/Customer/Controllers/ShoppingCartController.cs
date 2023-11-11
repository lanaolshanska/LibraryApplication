using Library.Utility;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Areas.Customer.Controllers
{
	[Area(Role.Customer)]
	public class ShoppingCartController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
