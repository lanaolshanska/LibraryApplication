using Library.Models;

namespace Library.DataAccess.Repository.Interfaces
{
	public interface IProductImageRepository : IRepository<ProductImage>
	{
		IEnumerable<ProductImage> GetProductImages(int productId);
	}
}
