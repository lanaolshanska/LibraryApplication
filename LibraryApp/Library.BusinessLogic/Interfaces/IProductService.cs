using Library.Models;
using Microsoft.AspNetCore.Http;

namespace Library.BusinessLogic.Interfaces
{
	public interface IProductService : IBaseService<Product>
	{
		void SaveFiles(List<IFormFile> files, Product product);
	}
}
