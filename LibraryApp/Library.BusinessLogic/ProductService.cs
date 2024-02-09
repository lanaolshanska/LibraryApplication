using Library.BusinessLogic.Interfaces;
using Library.DataAccess.Repository.Interfaces;
using Library.Models;
namespace Library.BusinessLogic
{
	public class ProductService : BaseService<Product>, IProductService
	{
		private readonly IProductImageService _imageService;
		public ProductService(IProductRepository repository, IProductImageService imageService) : base(repository)
		{
			_imageService = imageService;
		}

		public void Delete(Product product)
		{
			var productImages = product.Images.ToList();
			_imageService.DeleteAllProductImages(productImages);
			Delete(product.Id);
		}
	}
}
