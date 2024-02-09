using Library.BusinessLogic.Interfaces;
using Library.DataAccess.Repository.Interfaces;
using Library.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Library.BusinessLogic
{
	public class ProductImageService : BaseService<ProductImage>, IProductImageService
	{
		private readonly IProductImageRepository _repository;
		private readonly IWebHostEnvironment _webHostEnvironment;

		private const string imagesPath = @"images\product";

		public ProductImageService(IProductImageRepository repository, IWebHostEnvironment webHostEnvironment) : base(repository)
		{
			_repository = repository;
			_webHostEnvironment = webHostEnvironment;
		}
		public IEnumerable<ProductImage> GetProductImages(int productId)
		{
			return _repository.GetProductImages(productId);
		}

		public void SaveImages(List<IFormFile> files, int productId)
		{
			var fileHelper = new FileHelper();
			foreach (var file in files)
			{
				var filePath = fileHelper.SaveFile(file, imagesPath, _webHostEnvironment.WebRootPath);
				Create(new ProductImage
				{
					ImageUrl = filePath,
					ProductId = productId
				});
			}
			var productImages = GetProductImages(productId).ToList();
			var productCover = productImages.Where(x => x.IsCover).FirstOrDefault();
			if (productCover == null)
			{
				UpdateNextCover(productImages);
			};
		}

		public void SetCover(ProductImage image)
		{
			var previousCover = GetProductImages(image.ProductId)
								.Where(x => x.IsCover).FirstOrDefault();
			if (previousCover != null)
			{
				previousCover.IsCover = false;
				Update(previousCover);
			}
			image.IsCover = true;
			Update(image);
		}

		public void DeleteProductImage(ProductImage image)
		{
			if (image.IsCover == true)
			{
				var productImages = GetProductImages(image.ProductId)
									.Where(x => x.Id != image.Id).ToList();
				UpdateNextCover(productImages);
			}
			DeleteImage(image);
		}

		public void DeleteAllProductImages(List<ProductImage>? images)
		{
			images?.ForEach(x => DeleteImage(x));
		}

		private void UpdateNextCover(List<ProductImage>? productImages)
		{
			var newCover = productImages?.First();
			if (newCover != null)
			{
				newCover.IsCover = true;
				Update(newCover);
			}
		}

		private void DeleteImage(ProductImage image)
		{
			Delete(image.Id);
			var fileHelper = new FileHelper();
			fileHelper.DeleteFile(image.ImageUrl, _webHostEnvironment.WebRootPath);
		}
	}
}
