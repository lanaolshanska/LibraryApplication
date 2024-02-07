using Library.BusinessLogic.Interfaces;
using Library.DataAccess.Repository.Interfaces;
using Library.Models;

namespace Library.BusinessLogic
{
	public class ProductImageService : BaseService<ProductImage>, IProductImageService
	{
		public ProductImageService(IProductImageRepository repository) : base(repository)
		{
		}
	}
}
