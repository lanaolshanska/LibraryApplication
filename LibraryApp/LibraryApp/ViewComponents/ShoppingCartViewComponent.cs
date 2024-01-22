using Library.DataAccess.Repository.Interfaces;
using Library.Utility.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryApp.ViewComponents
{
	public class ShoppingCartViewComponent : ViewComponent
	{
		private readonly IShoppingCartRepository _shoppingCartRepository;

		public ShoppingCartViewComponent(IShoppingCartRepository shoppingCartRepository)
		{
			_shoppingCartRepository = shoppingCartRepository;
		}

		public async Task<IViewComponentResult> InvokeAsync() 
		{
			var userId = GetApplicationUserId();
			if (!string.IsNullOrEmpty(userId))
			{
				if (HttpContext.Session.GetInt32(Session.ShoppingCart) == null)
				{
					SetShoppingCartSession(userId);
				}
				return View(HttpContext.Session.GetInt32(Session.ShoppingCart));
			}
			else
			{
				HttpContext.Session.Clear();
				return View(0);
			}
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

		private void SetShoppingCartSession(string userId)
		{
			HttpContext.Session.SetInt32(Session.ShoppingCart,
				_shoppingCartRepository.GetByUserId(userId).Sum(x => x.Count));
		}
	}
}
