using Library.DataAccess.Data;
using Library.DataAccess.Repository.Interfaces;
using Library.Models;

namespace Library.DataAccess.Repository
{
	public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
	{
		public ProductImageRepository(ApplicationDbContext db) : base(db)
		{
		}

		public IEnumerable<ProductImage> GetProductImages(int productId)
		{
			return _dbSet.Where(x => x.ProductId == productId);
		}
	}
}
