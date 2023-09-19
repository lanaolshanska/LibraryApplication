namespace LibraryApp.Areas.Admin.Controllers
{
	using Library.DataAccess.Repository.Interfaces;
	using Microsoft.AspNetCore.Mvc;

	public class ProductController : Controller
	{
		private readonly IProductRepository _productRepository;
		public ProductController(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public IActionResult Index()
		{
			var products = _productRepository.GetAll();
			return View(products);
		}
	}
}
