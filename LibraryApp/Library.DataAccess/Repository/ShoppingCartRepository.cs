using Library.DataAccess.Data;
using Library.DataAccess.Repository.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repository
{
	public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
	{
		public ShoppingCartRepository(ApplicationDbContext db) : base(db)
		{
		}

        public IEnumerable<ShoppingCart> GetByUserId(string userId)
        {
            return _dbSet
                .Include(t => t.Product)
                .Where(x => x.ApplicationUserId == userId)
                .ToList();
        }
    }
}
