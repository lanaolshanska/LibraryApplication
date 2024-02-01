using Library.BusinessLogic.Interfaces;
using Library.Models.ViewModels;
using Library.Utility.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

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

		[HttpGet]
		public IActionResult Index()
		{
			var users = _userService.GetAll();
			return View(users);
		}

		[HttpGet]
		public IActionResult RoleManagement(string userId)
		{
			var roleManagementDetails = _userService.GetRoleManagementDetails(userId);
			if(roleManagementDetails != null)
			{
				return View(roleManagementDetails);
			}
			return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> RoleManagement(RoleManagementVM roleManagementDetails)
		{
			var userId = await _userService.UpdateRole(roleManagementDetails);
			if(userId != null) 
			{
				TempData["successMessage"] = "User was successfully updated!";
			}
			else
			{
				TempData["errorMessage"] = "Something went wrong!";
			}
			return RedirectToAction(nameof(Index));
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
