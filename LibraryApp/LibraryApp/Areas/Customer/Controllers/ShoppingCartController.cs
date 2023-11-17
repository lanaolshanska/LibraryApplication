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
	public class ShoppingCartController : Controller
	{
		private readonly IShoppingCartRepository _shoppingCartRepository;
		private readonly IApplicationUserRepository _userRepository;
		private readonly IUserAddressRepository _addressRepository;

		public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IApplicationUserRepository userRepository, IUserAddressRepository addressRepository)
		{
			_shoppingCartRepository = shoppingCartRepository;
			_userRepository = userRepository;
			_addressRepository = addressRepository;
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

		#region ApiCalls

		[HttpGet]
		public IActionResult GetAddress(int id)
		{
			var address = _addressRepository.GetById(1);
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
