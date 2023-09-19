namespace LibraryApp.Areas.Admin.Controllers
{
	using Library.DataAccess.Repository.Interfaces;
	using Library.Models;
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

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Product product)
		{
			if (ModelState.IsValid)
			{
				_productRepository.Create(product);
				TempData["successMessage"] = "Item was successfully created!";
				return RedirectToAction("Index");
			}
			return View();
		}

		public IActionResult Update(int? id)
		{
			if (id.HasValue)
			{
				var product = _productRepository.GetById(id.Value);
				if (product != null)
				{
					return View(product);
				}
			}
			return NotFound();
		}

		[HttpPost]
		public IActionResult Update(Product product)
		{
			if (ModelState.IsValid)
			{
				_productRepository.Update(product);
				TempData["successMessage"] = "Item was successfully updated!";
				return RedirectToAction("Index");
			}
			return View();
		}

		public IActionResult Delete(int? id)
		{
			if (id.HasValue)
			{
				_productRepository.Delete(id.Value);
				TempData["warningMessage"] = "Item was deleted!";
				return RedirectToAction("Index");
			}
			return NotFound();
		}
	}
}
