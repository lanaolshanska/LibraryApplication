namespace LibraryApp.Areas.Customer.Controllers
{
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
		private readonly IProductRepository _productRepository;
		private readonly IShoppingCartRepository _shoppingCartRepository;
		private readonly IApplicationUserRepository _userRepository;

		public string UserId { get => GetApplicationUserId(); }

		public HomeController(IProductRepository productRepository, IShoppingCartRepository shoppingCartRepository, IApplicationUserRepository userRepository)
		{
			_productRepository = productRepository;
			_shoppingCartRepository = shoppingCartRepository;
			_userRepository = userRepository;
		}

		public IActionResult Index()
		{
			if (!string.IsNullOrEmpty(UserId))
			{
				var user = _userRepository.GetById(UserId);
				SetShoppingCartSession();

				if (Discount.CompanyUser != 0)
				{
					ViewBag.CompanyId = user.CompanyId;
				}
			}

			var products = _productRepository.GetAll();
			return View(products);
		}

		public IActionResult Details(int id)
		{
			var shoppingCart = new ShoppingCart
			{
				ProductId = id,
				Product = _productRepository.GetAll().FirstOrDefault(p => p.Id == id),
				ApplicationUser = !string.IsNullOrEmpty(UserId) ? _userRepository.GetById(UserId) : null
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

		public IActionResult Privacy()
		{
			return View();
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
