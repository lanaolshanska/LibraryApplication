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

		public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IApplicationUserRepository userRepository)
		{
			_shoppingCartRepository = shoppingCartRepository;
			_userRepository = userRepository;
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

			var summaryViewModel = new SummaryVM
			{
				ProductList = products,
				OrderTotal = products.Sum(x => x.Count * x.Product.Price),
				Address = new UserAddress
				{
					Id = _userRepository.GetById(userId)?.Address?.Id ?? 0,
				}
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
	}
}
