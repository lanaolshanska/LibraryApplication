namespace LibraryApp.Areas.Customer.Controllers
{
	using Library.BusinessLogic.Interfaces;
	using Library.DataAccess.Repository.Interfaces;
	using Library.Models;
	using Library.Utility.Constants;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using System.Diagnostics;
	using System.Security.Claims;

	[Area(Role.Customer)]
	public class HomeController : Controller
	{
		private readonly IProductService _productService;
		private readonly IShoppingCartRepository _shoppingCartRepository;
		private readonly IUserService _userService;

		public string UserId { get => GetApplicationUserId(); }

		public HomeController(IProductService productService,
			IShoppingCartRepository shoppingCartRepository,
			IUserService userService)
		{
			_productService = productService;
			_shoppingCartRepository = shoppingCartRepository;
			_userService = userService;
		}

		public IActionResult Index()
		{
			if (!string.IsNullOrEmpty(UserId))
			{
				var user = _userService.GetById(UserId);
				if (user != null && Discount.CompanyUser != 0)
				{
					ViewBag.CompanyId = user.CompanyId;
				}
			}
			var products = _productService.GetAll();
			return View(products);
		}

		public IActionResult Details(int id)
		{
			var shoppingCart = new ShoppingCart
			{
				ProductId = id,
				Product = _productService.GetById(id),
				ApplicationUser = !string.IsNullOrEmpty(UserId) ? _userService.GetById(UserId) : null
			};
			return View(shoppingCart);
		}

		[HttpPost]
		[Authorize]
		public IActionResult Details(ShoppingCart shoppingCart)
		{
			shoppingCart.ApplicationUserId = UserId;

			var userShoppingCart = _shoppingCartRepository.GetWhere(x =>
				x.ApplicationUserId == UserId &&
				x.ProductId == shoppingCart.ProductId)
				.FirstOrDefault();

			if (userShoppingCart == null)
			{
				_shoppingCartRepository.Create(shoppingCart);
			}
			else
			{
				userShoppingCart.Count += shoppingCart.Count;
				_shoppingCartRepository.Update(userShoppingCart);
			}
			SetShoppingCartSession();
			TempData["successMessage"] = "Cart updated successfully!";
			return RedirectToAction(nameof(Index));
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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

		private void SetShoppingCartSession()
		{
			HttpContext.Session.SetInt32(Session.ShoppingCart,
				_shoppingCartRepository.GetByUserId(UserId).Sum(x => x.Count));
		}
	}
}
