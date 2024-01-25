using Library.BusinessLogic.Interfaces;
using Library.BusinessLogic.Payments;
using Library.DataAccess.Repository.Interfaces;
using Library.Models;
using Library.Models.ViewModels;
using Library.Utility.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Security.Claims;

namespace LibraryApp.Areas.Customer.Controllers
{
    [Area(Role.Customer)]
	[Authorize]
	public class ShoppingCartController : Controller
	{
		private readonly IShoppingCartRepository _shoppingCartRepository;
		private readonly IUserService _userService;
		private readonly IPaymentService _paymentService;
		private readonly IAddressService _addressService;
		private readonly IOrderService _orderService;

		public string UserId { get => GetApplicationUserId(); }

		public ShoppingCartController(IShoppingCartRepository shoppingCartRepository,
									IUserService userService,
									IPaymentService paymentService,
									IAddressService addressService,
									IOrderService orderService
									)
		{
			_shoppingCartRepository = shoppingCartRepository;
			_userService = userService;
			_paymentService = paymentService;
			_addressService = addressService;
			_orderService = orderService;
		}

		public IActionResult Index()
		{
			var userShoppingCarts = _shoppingCartRepository.GetByUserId(UserId);
			var shoppingCartModel = new ShoppingCartVM
			{
				ShoppingCartList = userShoppingCarts,
				OrderTotal = userShoppingCarts.Sum(x => x.Count * x.Product.Price),
				ApplicationUser = _userService.GetById(UserId)
			};

			if (shoppingCartModel.ApplicationUser.CompanyId.HasValue &&
				Discount.CompanyUser != 0)
			{
				shoppingCartModel.OrderTotal = _orderService.CalculateDiscountOrderTotal(shoppingCartModel.OrderTotal, Discount.CompanyUser);
			}

			return View(shoppingCartModel);
		}

		public IActionResult Summary()
		{
			var products = _shoppingCartRepository.GetByUserId(UserId);
			var primaryAddressId = _addressService.GetPrimaryUserAddress(UserId)?.Id;

			var summaryViewModel = new SummaryVM
			{
				ProductList = products,
				OrderTotal = products.Sum(x => x.Count * x.Product.Price),
				Address = new UserAddress { Id = primaryAddressId ?? 0 },
				ApplicationUser = _userService.GetById(UserId)
			};
			if (summaryViewModel.ApplicationUser.CompanyId.HasValue &&
				Discount.CompanyUser != 0)
			{
				summaryViewModel.OrderTotal = _orderService.CalculateDiscountOrderTotal(summaryViewModel.OrderTotal, Discount.CompanyUser);
			}
			return View(summaryViewModel);
		}

		[HttpPost]
		public IActionResult CreateOrder(SummaryVM summaryViewModel)
		{
			if (ModelState.IsValid)
			{
				var user = _userService.GetById(UserId);
				var shoppingCarts = _shoppingCartRepository.GetByUserId(UserId).ToList();

				var order = _orderService.CreateOrder(user, summaryViewModel.Address, shoppingCarts);

				var successUrl = $"Customer/ShoppingCart/OrderConfirmation?id={order.Id}";
				var cancelUrl = "Customer/ShoppingCart/Summary";

				var stripeService = new StripeService();
				var stripeSession = stripeService.CreateStripeSession(successUrl, cancelUrl, order.Products);

				_paymentService.UpdateStripePaymentDetails(order.PaymentDetailId, stripeSession.Id, stripeSession.PaymentIntentId);

				Response.Headers.Add("Location", stripeSession.Url);
				return new StatusCodeResult(303);

			}
			TempData["errorMessage"] = "Address is not valid!";
			return RedirectToAction(nameof(Summary));
		}

		public IActionResult OrderConfirmation(int id)
		{
			var order = _orderService.GetById(id);
			var payment = _paymentService.GetById(order.PaymentDetailId);

			if (payment.Status != PaymentStatus.Delayed &&
				!string.IsNullOrEmpty(payment.SessionId))
			{
				_paymentService.ApprovePayment(payment.Id, payment.SessionId);
				_orderService.UpdateStatus(id, OrderStatus.Approved);
			}
			var oldShoppingCarts = _shoppingCartRepository.GetByUserId(order.ApplicationUserId).ToList();
			_shoppingCartRepository.RemoveRange(oldShoppingCarts);
			SetShoppingCartSession();

			return View(id);
		}

		public IActionResult Plus(int id)
		{
			var order = _shoppingCartRepository.GetById(id);
			if (order != null)
			{
				order.Count++;
				_shoppingCartRepository.Update(order);
				SetShoppingCartSession();
			}
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Minus(int id)
		{
			var order = _shoppingCartRepository.GetById(id);
			if (order != null)
			{
				if (order.Count > 1)
				{
					order.Count--;
					_shoppingCartRepository.Update(order);
				}
				else
				{
					_shoppingCartRepository.Delete(id);
				}
				SetShoppingCartSession();
			}
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Delete(int id)
		{
			var order = _shoppingCartRepository.GetById(id);
			if (order != null)
			{
				_shoppingCartRepository.Delete(id);
				SetShoppingCartSession();
			}
			return RedirectToAction(nameof(Index));
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

		#region ApiCalls

		[HttpGet]
		public IActionResult GetAddress(int id)
		{
			var address = _addressService.GetById(id);
			if (address != null)
			{
				return Json(new { success = true, address = address });
			}
			else
			{
				return Json(new { success = false, message = "Can not retrieve address!" });
			}
		}
		private void SetShoppingCartSession()
		{
			HttpContext.Session.SetInt32(Session.ShoppingCart,
				_shoppingCartRepository.GetByUserId(UserId).Sum(x => x.Count));
		}
		#endregion
	}
}
