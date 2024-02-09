namespace LibraryApp.Areas.Admin.Controllers
{
	using Library.BusinessLogic;
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
		private readonly IProductImageService _productImageService;
		private readonly ICategoryRepository _categoryRepository;

		public ProductController(IProductService productService,
			IProductImageService productImageService,
			ICategoryRepository categoryRepository)
		{
			_productService = productService;
			_categoryRepository = categoryRepository;
			_productImageService = productImageService;
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
					_productImageService.SaveImages(files, productVm.Product.Id);
				}
			}
			productVm.Categories = _categoryRepository.GetCategoriesList();
			productVm.Product.Images = _productImageService.GetProductImages(productVm.Product.Id).ToList();
			return View(productVm);
		}

		[HttpPost]
		public IActionResult SetCover(int id)
		{
			var image = _productImageService.GetById(id);
			if (image != null)
			{
				_productImageService.SetCover(image);
				return RedirectToAction(nameof(CreateOrUpdate), new { id = image.ProductId });
			}
			return NotFound();
		}

		[HttpPost]
		public IActionResult DeleteImage(int id)
		{
			var image = _productImageService.GetById(id);
			if (image != null)
			{
				_productImageService.DeleteProductImage(image);
				return RedirectToAction(nameof(CreateOrUpdate), new { id = image.ProductId });
			}
			return NotFound();
		}

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
			else
			{
				_productService.Delete(product);
				return Json(new { success = true, message = "Item was deleted!" });
			}
		}
	}

	#endregion
}
