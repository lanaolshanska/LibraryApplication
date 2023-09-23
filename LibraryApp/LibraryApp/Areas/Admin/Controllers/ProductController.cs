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
		private readonly IWebHostEnvironment _webHostEnvironment;

		private const string imagesPath = @"images\product";
		public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, IWebHostEnvironment webHostEnvironment)
		{
			_productRepository = productRepository;
			_categoryRepository = categoryRepository;
			_webHostEnvironment = webHostEnvironment;
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
				productVm.Product.ImageUrl = SaveFile(file, productVm.Product.ImageUrl);

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

		private string SaveFile(IFormFile? file, string imageUrl)
		{
			if (file != null)
			{
				if (!string.IsNullOrEmpty(imageUrl))
				{
					var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl);
					if (System.IO.File.Exists(oldFilePath))
					{
						System.IO.File.Delete(oldFilePath);
					}
				}
				var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
				var filePath = Path.Combine(imagesPath, fileName);
				var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, filePath);
				using (var fileStream = new FileStream(fullPath, FileMode.Create))
				{
					file.CopyTo(fileStream);
				}
				return filePath;
			}
			return imageUrl;
		}
	}
}
