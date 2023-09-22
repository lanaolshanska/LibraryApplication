namespace LibraryApp.Areas.Admin.Controllers
{
	using Library.DataAccess.Repository.Interfaces;
	using Library.Models;
	using Library.Models.ViewModels;
	using Microsoft.AspNetCore.Mvc;

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

		public IActionResult CreateOrUpdate(int? id)
		{
			var productVm = new ProductViewModel();
			productVm.Categories = _categoryRepository.GetCategoriesList();
			productVm.Product = id.HasValue ? _productRepository.GetById(id.Value) : new Product();
			return View(productVm);
		}

		[HttpPost]
		public IActionResult CreateOrUpdate(ProductViewModel productVm, IFormFile? file)
		{
			if (ModelState.IsValid)
			{
				if (productVm.Product.Id == 0)
				{
					_productRepository.Create(productVm.Product);
					TempData["successMessage"] = "Item was successfully created!";
					
				}
				else
				{
					_productRepository.Update(productVm.Product);
					TempData["successMessage"] = "Item was successfully updated!";
				}
				return RedirectToAction("Index");
			}
			else
			{
				productVm.Categories = _categoryRepository.GetCategoriesList();
				return View(productVm);
			}
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
