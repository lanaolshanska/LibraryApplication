namespace LibraryApp.Areas.Admin.Controllers
{
	using Library.DataAccess.Repository.Interfaces;
	using Library.Models;
	using Library.Models.ViewModels;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Rendering;

	public class ProductController : Controller
	{
		private readonly IProductRepository _productRepository;
		private readonly ICategoryRepository _categoryRepository;
		public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
		{
			_productRepository = productRepository;
			_categoryRepository = categoryRepository;
		}

		public IActionResult Index()
		{
			var products = _productRepository.GetAll();
			return View(products);
		}

		public IActionResult Create()
		{
			var productVm = new ProductViewModel();
			productVm.Categories = _categoryRepository.GetCategoriesList();
			return View(productVm);
		}

		[HttpPost]
		public IActionResult Create(ProductViewModel productVm)
		{
			if (ModelState.IsValid)
			{
				_productRepository.Create(productVm.Product);
				TempData["successMessage"] = "Item was successfully created!";
				return RedirectToAction("Index");
			}
			else
			{
				productVm.Categories = _categoryRepository.GetCategoriesList();
				return View(productVm);
			}
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
