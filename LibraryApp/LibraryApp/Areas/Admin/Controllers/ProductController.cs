namespace LibraryApp.Areas.Admin.Controllers
{
	using Library.DataAccess.Repository.Interfaces;
	using Library.Models;
	using Library.Models.ViewModels;
	using Library.Utility.Constants;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Area(Role.Admin)]
	[Authorize(Roles = Role.Admin)]
	public class ProductController : Controller
	{
		private readonly IProductRepository _productRepository;
		private readonly ICategoryRepository _categoryRepository;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IProductImageRepository _productImageRepository;

		private const string imagesPath = @"images\product";

		public ProductController(IProductRepository productRepository, 
			ICategoryRepository categoryRepository, 
			IWebHostEnvironment webHostEnvironment,
			IProductImageRepository productImageRepository)
		{
			_productRepository = productRepository;
			_categoryRepository = categoryRepository;
			_webHostEnvironment = webHostEnvironment;
			_productImageRepository = productImageRepository;
		}

		public IActionResult Index()
		{
			var products = _productRepository.GetAll();
			return View(products);
		}

		public IActionResult CreateOrUpdate(int? id)
		{
			var productVm = new ProductVM
			{
				Categories = _categoryRepository.GetCategoriesList(),
				Product = id.HasValue ? _productRepository.GetById(id.Value) : new Product()
			};
			return View(productVm);
		}

		[HttpPost]
		public IActionResult CreateOrUpdate(ProductVM productVm, List<IFormFile> files)
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

				if (files != null)
					SaveFiles(files, productVm.Product.Id);

				return RedirectToAction("Index");
			}
			else
			{
				productVm.Categories = _categoryRepository.GetCategoriesList();
				return View(productVm);
			}
		}

		private void SaveFiles(List<IFormFile> files, int productId)
		{
			foreach (var file in files)
			{
				var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
				var filePath = Path.Combine(imagesPath, fileName);
				var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, filePath);

				using (var fileStream = new FileStream(fullPath, FileMode.Create))
				{
					file.CopyTo(fileStream);
				}

				var productImage = new ProductImage
				{
					ImageUrl = filePath,
					ProductId = productId
				};
				_productImageRepository.Create(productImage);
			}
		}

		//private void DeleteOldFile(string imageUrl)
		//{
		//	var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl);
		//	if (System.IO.File.Exists(oldFilePath))
		//	{
		//		System.IO.File.Delete(oldFilePath);
		//	}
		//}

		#region ApiCalls

		[HttpGet]
		public IActionResult GetAll()
		{
			var products = _productRepository.GetAll();
			return Json(new { data = products });
		}

		[HttpDelete]
		public IActionResult Delete(int id)
		{
			var product = _productRepository.GetById(id);
			if (product == null)
			{
				return Json(new { success = false, message = "Item for deleting not found!" });
			}

			//if (!string.IsNullOrEmpty(product.ImageUrl))
			//	DeleteOldFile(product.ImageUrl);

			_productRepository.Delete(id);
			return Json(new { success = true, message = "Item was deleted!" });
		}
	}

	#endregion
}
