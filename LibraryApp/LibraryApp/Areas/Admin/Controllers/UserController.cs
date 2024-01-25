using Library.BusinessLogic.Interfaces;
using Library.Utility.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Areas.Admin.Controllers
{
	[Area(Role.Admin)]
	[Authorize(Roles = Role.Admin)]
	public class UserController : Controller
	{
		private readonly IUserService _userService;
		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		public IActionResult Index()
		{
			var users = _userService.GetAll();
			return View(users);
		}

		#region ApiCalls

		public IActionResult GetAll()
		{
			var users = _userService.GetAll();
			return Json(new { data = users });
		}

		#endregion
	}
}
