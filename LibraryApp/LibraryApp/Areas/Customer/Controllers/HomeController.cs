namespace LibraryApp.Areas.Customer.Controllers
{
	using Library.DataAccess.Repository.Interfaces;
	using Library.Models;
    using Library.Utility;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
	using System.Security.Claims;

	[Area(Role.Customer)]
    public class HomeController : Controller
    {
		private readonly IProductRepository _productRepository;
		private readonly IShoppingCartRepository _shoppingCartRepository;

		public HomeController(IProductRepository productRepository, IShoppingCartRepository shoppingCartRepository)
        {
            _productRepository = productRepository;
            _shoppingCartRepository = shoppingCartRepository;
		}

        public IActionResult Index()
        {
            var products = _productRepository.GetAll();
            return View(products);
        }

		public IActionResult Details(int id)
		{
            var shoppingCart = new ShoppingCart
            {
				ProductId = id,
				Product = _productRepository.GetAll().FirstOrDefault(p => p.Id == id)
			};
			return View(shoppingCart);
		}

        [HttpPost]
        [Authorize]
		public IActionResult Details(ShoppingCart shoppingCart)
		{
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
			shoppingCart.ApplicationUserId = userId;

			var userShoppingCart = _shoppingCartRepository.GetWhere(x => 
                x.ApplicationUserId == userId && 
                x.ProductId == shoppingCart.ProductId)
                .FirstOrDefault();

            if(userShoppingCart == null)
            {
				_shoppingCartRepository.Create(shoppingCart);
            }
            else
            {
				userShoppingCart.Count += shoppingCart.Count;
				_shoppingCartRepository.Update(userShoppingCart);

			}
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
    }
}
