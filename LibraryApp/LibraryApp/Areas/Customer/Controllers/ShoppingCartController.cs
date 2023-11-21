using Library.DataAccess.Repository.Interfaces;
using Library.Models;
using Library.Models.ViewModels;
using Library.Utility;
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
		private readonly IApplicationUserRepository _userRepository;
		private readonly IUserAddressRepository _addressRepository;
		private readonly IShipmentDetailRepository _shipmentRepository;
		private readonly IPaymentDetailRepository _paymentRepository;
		private readonly IOrderProductRepository _orderProductRepository;
		private readonly IOrderRepository _orderRepository;

		[BindProperty]
		public SummaryVM OrderSummary { get; set; }

		public ShoppingCartController(IShoppingCartRepository shoppingCartRepository,
									IApplicationUserRepository userRepository,
									IUserAddressRepository addressRepository,
									IShipmentDetailRepository shipmentRepository,
									IPaymentDetailRepository paymentRepository,
									IOrderProductRepository orderProductRepository,
									IOrderRepository orderRepository)
		{
			_shoppingCartRepository = shoppingCartRepository;
			_userRepository = userRepository;
			_addressRepository = addressRepository;
			_shipmentRepository = shipmentRepository;
			_paymentRepository = paymentRepository;
			_orderProductRepository = orderProductRepository;
			_orderRepository = orderRepository;
		}

		public IActionResult Index()
		{
			var userId = GetApplicationUserId();
			var userShoppingCarts = _shoppingCartRepository.GetByUserId(userId);

			var shoppingCartModel = new ShoppingCartVM
			{
				ShoppingCartList = userShoppingCarts,
				OrderTotal = userShoppingCarts.Sum(x => x.Count * x.Product.Price)
			};

			return View(shoppingCartModel);
		}

		public IActionResult Summary()
		{
			var userId = GetApplicationUserId();
			var products = _shoppingCartRepository.GetByUserId(userId);
			var primaryAddressId = _addressRepository.GetPrimaryUserAddress(userId)?.Id;

			var summaryViewModel = new SummaryVM
			{
				ProductList = products,
				OrderTotal = products.Sum(x => x.Count * x.Product.Price),
				Address = new UserAddress { Id = primaryAddressId ?? 0 }
			};
			return View(summaryViewModel);
		}

		[HttpPost]
		public IActionResult CreateOrder(SummaryVM summaryViewModel)
		{
			if (ModelState.IsValid)
			{
				var userId = GetApplicationUserId();
				var user = _userRepository.GetById(userId);

				var addressId = CreateOrUpdateAddress(summaryViewModel.Address, userId);

				var shipmentDetail = new ShipmentDetail { UserAddressId = addressId };
				_shipmentRepository.Create(shipmentDetail);

				var paymentDetail = new PaymentDetail { Status = user.CompanyId.HasValue ? PaymentStatus.Delayed : PaymentStatus.Pending };
				_paymentRepository.Create(paymentDetail);

				var shoppingCarts = _shoppingCartRepository.GetByUserId(userId).ToList();

				var order = new Order
				{
					Date = DateTime.Now,
					Total = shoppingCarts.Sum(x => x.Count * x.Product.Price),
					Status = user.CompanyId.HasValue ? OrderStatus.Approved : OrderStatus.Pending,
					ApplicationUserId = userId,
					ShipmentDetailId = shipmentDetail.Id,
					PaymentDetailId = paymentDetail.Id
				};

				_orderRepository.Create(order);

				shoppingCarts.ForEach(shoppingCart =>
				{
					_orderProductRepository.Create(
							new OrderProduct
							{
								OrderId = order.Id,
								ProductId = shoppingCart.ProductId,
								Count = shoppingCart.Count,
								Price = shoppingCart.Product.Price,
							});
					_shoppingCartRepository.Delete(shoppingCart.Id);
				});
				TempData["successMessage"] = "Order was successfully created!";
				return RedirectToAction("Index", "Home");
			}
			TempData["warningMessage"] = "Address is not valid!";
			return RedirectToAction(nameof(Summary));
		}

		public IActionResult Plus(int id)
		{
			var order = _shoppingCartRepository.GetById(id);
			if (order != null)
			{
				order.Count++;
				_shoppingCartRepository.Update(order);
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
			}
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Delete(int id)
		{
			var order = _shoppingCartRepository.GetById(id);
			if (order != null)
			{
				_shoppingCartRepository.Delete(id);
			}
			return RedirectToAction(nameof(Index));
		}

		private string GetApplicationUserId()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
			return userId;
		}

		private int CreateOrUpdateAddress(UserAddress newAddress, string userId)
		{
			int primaryAddressId;
			var (isAddressUnique, existingAddress) = _addressRepository.IsUnique(newAddress, userId);
			if (isAddressUnique)
			{
				newAddress.ApplicationUserId = userId;
				_addressRepository.Create(newAddress);
				primaryAddressId = newAddress.Id;
			}
			else
			{
				primaryAddressId = existingAddress.Id;
			}
			_addressRepository.SetPrimaryAddress(primaryAddressId, userId);
			return primaryAddressId;
		}

		#region ApiCalls

		[HttpGet]
		public IActionResult GetAddress(int id)
		{
			var address = _addressRepository.GetById(id);
			if (address != null)
			{
				return Json(new { success = true, address = address });
			}
			else
			{
				return Json(new { success = false, message = "Can not retrieve address!" });
			}
		}
		#endregion
	}
}
