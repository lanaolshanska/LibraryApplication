using Library.DataAccess.Data;
using Library.DataAccess.Repository.Interfaces;
using Library.Models;

namespace Library.DataAccess.Repository
{
	public class OrderRepository : Repository<Order>, IOrderRepository
	{
		public OrderRepository(ApplicationDbContext db) : base(db)
		{
		}
	}
}
