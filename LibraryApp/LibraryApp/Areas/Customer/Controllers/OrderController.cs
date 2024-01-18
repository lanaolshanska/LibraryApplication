using Library.BusinessLogic;
using Library.BusinessLogic.Interfaces;
using Library.BusinessLogic.Payments;
using Library.DataAccess.Repository;
using Library.DataAccess.Repository.Interfaces;
using Library.Models;
using Library.Models.ViewModels;
using Library.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryApp.Areas.Customer.Controllers
{
	[Area(Role.Customer)]
	[Authorize]
	public class OrderController : Controller
	{
		private readonly IOrderService _orderService;
		private readonly IPaymentService _paymentService;
		private readonly IOrderProductRepository _orderProductRepository;

		public OrderController(IOrderService orderService, IPaymentService paymentService, IOrderProductRepository orderProductRepository)
		{
			_orderService = orderService;
			_paymentService = paymentService;
			_orderProductRepository = orderProductRepository;
		}

		[HttpGet]
		public IActionResult Index()
		{
			var userId = GetApplicationUserId();
			var orders = _orderService.GetOrdersByCustomerId(userId).ToList();
			return View(orders);
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
		public IActionResult ProcessPayment(OrderDetailsVM orderVm)
		{
			var order = _orderService.GetDetailsById(orderVm.Id);

			var successUrl = $"Customer/ShoppingCart/OrderConfirmation?id={order.Id}";
			var cancelUrl = $"Customer/Order/Details/{order.Id}";

			var stripeService = new StripeService();
			var stripeSession = stripeService.CreateStripeSession(successUrl, cancelUrl, order.Products);

			_paymentService.UpdateStripePaymentDetails(order.PaymentDetail.Id, stripeSession.Id, stripeSession.PaymentIntentId);

			Response.Headers.Add("Location", stripeSession.Url);
			return new StatusCodeResult(303);
		}

		[HttpPost]
		public IActionResult CancelOrder(OrderDetailsVM orderVm)
		{
			var order = _orderService.GetById(orderVm.Id);
			var payment = _paymentService.GetById(order.PaymentDetailId);

			_paymentService.UpdateStatus(payment.Id, PaymentStatus.Cancelled);
			_orderService.UpdateStatus(order.Id, OrderStatus.Cancelled);

			return RedirectToAction(nameof(Details), new { id = order.Id });
		}

		private string GetApplicationUserId()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			if (claimsIdentity.IsAuthenticated)
			{
				return claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
			}
			return string.Empty;
		}
	}
}
