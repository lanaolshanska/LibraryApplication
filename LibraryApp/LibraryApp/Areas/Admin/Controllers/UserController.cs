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

		[HttpGet]
		public IActionResult GetAll()
		{
			var users = _userService.GetAll();
			return Json(new { data = users });
		}

		[HttpPost]
		public IActionResult ToggleLock([FromBody] string id)
		{
			var user = _userService.GetById(id);
			if (user != null)
			{
				if (user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
				{
					user.LockoutEnd = DateTime.Now;
				}
				else
				{
					user.LockoutEnd = DateTime.Now.AddYears(100);
				}
				_userService.Update(user);
				return Json(new { success = true, message = "Operation successful!" });
			}
			return Json(new { success = false, message = "Something went wrong!" });
		}

		#endregion
	}
}
