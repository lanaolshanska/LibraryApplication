using Library.BusinessLogic.Interfaces;
using Library.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Areas.Admin.Controllers
{
	[Area(Role.Admin)]
	[Authorize]
	public class OrderController : Controller
	{
		private readonly IOrderService _orderService;

		public OrderController(IOrderService orderService)
		{
			_orderService = orderService;
		}

		public IActionResult Index()
		{
			var orderStatus = typeof(OrderStatus);
			var statuses = orderStatus.GetFields()
									  .Select(field => (string)field.GetValue(null))
									  .ToList();
			return View(statuses);
		}

		#region ApiCalls

		public IActionResult GetAll(string? status)
		{
			var orders = _orderService.GetAll();
			if (!string.IsNullOrEmpty(status))
			{
				orders = orders.Where(x => x.Status.ToLower() == status);
			}
			return Json(new { data = orders });
		}

		#endregion
	}
}
