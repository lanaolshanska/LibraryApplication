using Library.Models;
using Microsoft.AspNetCore.Http;

namespace Library.BusinessLogic.Interfaces
{
	public interface IProductImageService : IBaseService<ProductImage>
	{
		IEnumerable<ProductImage> GetProductImages(int productId);
		void SaveImages(List<IFormFile> files, int productId);
		void SetCover(ProductImage image);
		void DeleteImage(ProductImage image);
	}
}
