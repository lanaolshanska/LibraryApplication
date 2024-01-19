using Library.BusinessLogic.Interfaces;
using Library.BusinessLogic.Payments;
using Library.Models.ViewModels;
using Library.Utility.Constants;
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
		private readonly IShipmentDetailService _shipmentDetailService;

		[BindProperty]
		public OrderDetailsVM OrderDetails { get; set; }

		public OrderController(IOrderService orderService, IPaymentService paymentService, IShipmentDetailService shipmentDetailService)
		{
			_orderService = orderService;
			_paymentService = paymentService;
			_shipmentDetailService = shipmentDetailService;
		}

		public IActionResult Index()
		{
			var orderStatus = typeof(OrderStatus);
			var statuses = orderStatus.GetFields()
									  .Select(field => (string)field.GetValue(null))
									  .ToList();
			return View(statuses);
		}

		[HttpGet]
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
		public IActionResult UpdateShipmentDetails(ShipmentDetailVM shipmentDetail)
		{
			if (!string.IsNullOrEmpty(shipmentDetail.Carrier) &&
				!string.IsNullOrEmpty(shipmentDetail.TrackingNumber))
			{
				var shipmentDetailId = _shipmentDetailService.UpdateShipmentDetail(shipmentDetail);
				if (shipmentDetailId != null)
				{
					_orderService.UpdateStatus(shipmentDetail.OrderId, OrderStatus.Shipped);
				}
			}
			return RedirectToAction(nameof(Details), new { id = shipmentDetail.OrderId });
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

		[HttpGet]
		public IActionResult GetShipmentModal(int orderId)
		{
			return PartialView("_ShipmentModal", orderId);
		}

		#region ApiCalls
		[HttpGet]
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
