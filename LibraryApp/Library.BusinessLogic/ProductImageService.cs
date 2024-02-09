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
		}

		public void SetCover(ProductImage image)
		{
			var previousCover = _repository.GetProductImages(image.ProductId)
										.Where(x => x.IsCover).FirstOrDefault();
			if (previousCover != null)
			{
				previousCover.IsCover = false;
				Update(previousCover);
			}
			image.IsCover = true;
			Update(image);
		}

		public void DeleteImage(ProductImage image)
		{
			Delete(image.Id);
			var fileHelper = new FileHelper();
			fileHelper.DeleteFile(image.ImageUrl, _webHostEnvironment.WebRootPath);
		}
	}
}
