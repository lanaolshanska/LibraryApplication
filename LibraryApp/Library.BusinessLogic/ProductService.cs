using Library.BusinessLogic.Interfaces;
using Library.DataAccess.Repository.Interfaces;
using Library.Models;
namespace Library.BusinessLogic
{
	public class ProductService : BaseService<Product>, IProductService
	{
		public ProductService(IProductRepository repository) : base(repository)
		{
		}
	}
}
