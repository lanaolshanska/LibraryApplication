namespace LibraryApp.Areas.Customer.Controllers
{
	using Library.DataAccess.Repository.Interfaces;
	using Library.Models;
	using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;

    [Area("Customer")]
    public class HomeController : Controller
    {
		private readonly IProductRepository _productRepository;

		public HomeController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            var products = _productRepository.GetAll();
            return View(products);
        }

		public IActionResult Details(int id)
		{
			var product = _productRepository.GetAll().Where(p => p.Id == id).FirstOrDefault();
			return View(product);
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
