using Library.DataAccess.Data;
using Library.DataAccess.Repository.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repository
{
	public class OrderRepository : Repository<Order>, IOrderRepository
	{
		public OrderRepository(ApplicationDbContext db) : base(db)
		{
		}

		public override IEnumerable<Order> GetAll()
        {
			return _dbSet
				.Include(t => t.ApplicationUser)
				.Include(t => t.ShipmentDetail)
				.ThenInclude(x => x.UserAddress);
		}
	}
}
