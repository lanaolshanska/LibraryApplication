using Library.BusinessLogic.Interfaces;
using Library.DataAccess.Repository.Interfaces;
using Library.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Library.BusinessLogic
{
	public class ProductService : BaseService<Product>, IProductService
	{
		private readonly IWebHostEnvironment _webHostEnvironment;

		private const string imagesPath = @"images\product";

		public ProductService(IProductRepository repository, IWebHostEnvironment webHostEnvironment) : base(repository)
		{
			_webHostEnvironment = webHostEnvironment;
		}

		public void SaveFiles(List<IFormFile> files, Product product)
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
				product.Images.Add(new ProductImage
				{
					ImageUrl = filePath,
					ProductId = product.Id
				});
				product.ImageUrl = filePath;
			}
			Update(product);
		}
	}
}
