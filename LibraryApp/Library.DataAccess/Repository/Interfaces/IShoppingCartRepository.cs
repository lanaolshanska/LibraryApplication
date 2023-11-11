using Library.Models;

namespace Library.DataAccess.Repository.Interfaces
{
	public interface IShoppingCartRepository : IRepository<ShoppingCart>
	{
        IEnumerable<ShoppingCart> GetByUserId(string userId);

    }
}
