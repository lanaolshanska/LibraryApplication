namespace LibraryApp.Areas.Admin.Controllers
{
	using Library.BusinessLogic.Interfaces;
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
		private readonly IProductService _productService;
		private readonly ICategoryRepository _categoryRepository;
		private readonly IProductImageRepository _productImageRepository;

		public ProductController(IProductService productService,
			ICategoryRepository categoryRepository,
			IProductImageRepository productImageRepository)
		{
			_productService = productService;
			_categoryRepository = categoryRepository;
			_productImageRepository = productImageRepository;
		}

		public IActionResult Index()
		{
			var products = _productService.GetAll();
			return View(products);
		}

		public IActionResult CreateOrUpdate(int? id)
		{
			var productVm = new ProductVM
			{
				Categories = _categoryRepository.GetCategoriesList(),
				Product = id.HasValue ? _productService.GetById(id.Value) : new Product()
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
					_productService.Create(productVm.Product);
					TempData["successMessage"] = "Item was successfully created!";
				}
				else
				{
					_productService.Update(productVm.Product);
					TempData["successMessage"] = "Item was successfully updated!";
				}

				if (files != null)
				{
					_productService.SaveFiles(files, productVm.Product);
				}
			}
			productVm.Categories = _categoryRepository.GetCategoriesList();
			return View(productVm);
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
			var products = _productService.GetAll();
			return Json(new { data = products });
		}

		[HttpDelete]
		public IActionResult Delete(int id)
		{
			var product = _productService.GetById(id);
			if (product == null)
			{
				return Json(new { success = false, message = "Item for deleting not found!" });
			}

			//if (!string.IsNullOrEmpty(product.ImageUrl))
			//	DeleteOldFile(product.ImageUrl);

			_productService.Delete(id);
			return Json(new { success = true, message = "Item was deleted!" });
		}
	}

	#endregion
}
