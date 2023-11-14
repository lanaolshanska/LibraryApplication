using Library.DataAccess.Repository.Interfaces;
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

		public ShoppingCartController(IShoppingCartRepository shoppingCartRepository)
		{
			_shoppingCartRepository = shoppingCartRepository;
		}
		public IActionResult Index()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
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
			return View();
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
	}
}
