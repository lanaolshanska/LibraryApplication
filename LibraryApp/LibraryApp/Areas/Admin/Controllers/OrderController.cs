using Library.BusinessLogic.Interfaces;
using Library.BusinessLogic.Payments;
using Library.Models.ViewModels;
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
		private readonly IPaymentService _paymentService;

		[BindProperty]
		public OrderDetailsVM OrderDetails { get; set; }

		public OrderController(IOrderService orderService, IPaymentService paymentService)
		{
			_orderService = orderService;
			_paymentService = paymentService;
		}

		public IActionResult Index()
		{
			var orderStatus = typeof(OrderStatus);
			var statuses = orderStatus.GetFields()
									  .Select(field => (string)field.GetValue(null))
									  .ToList();
			return View(statuses);
		}

		public IActionResult Details(int id)
		{
			var orderDetails = _orderService.GetDetailsById(id);
			if (orderDetails != null)
			{
				return View(orderDetails);
			}
			else
			{
				return NotFound();
			}
		}

		[HttpPost]
		public IActionResult ProcessOrder()
		{
			_orderService.UpdateStatus(OrderDetails.Id, OrderStatus.Processing);
			return RedirectToAction(nameof(Details), new { id = OrderDetails.Id });
		}

		[HttpPost]
		public IActionResult CancelOrder()
		{
			var order = _orderService.GetById(OrderDetails.Id);
			var payment = _paymentService.GetById(order.PaymentDetailId);

			if (payment.Status == PaymentStatus.Approved)
			{
				var stripeService = new StripeService();
				stripeService.RefundPayment(payment.PaymentIntentId);

				_paymentService.UpdateStatus(payment.Id, PaymentStatus.Refunded);
			}
			else
			{
				_paymentService.UpdateStatus(payment.Id, PaymentStatus.Cancelled);
			}
			_orderService.UpdateStatus(order.Id, OrderStatus.Cancelled);
			return RedirectToAction(nameof(Details), new { id = order.Id });
		}

		//public IActionResult GetShipmentModal()
		//{
		//	return PartialView("_ShipmentModal");
		//}

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
