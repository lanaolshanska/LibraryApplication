using Library.DataAccess.Data;
using Library.DataAccess.Repository.Interfaces;
using Library.Models;

namespace Library.DataAccess.Repository
{
	public class OrderProductRepository : Repository<OrderProduct>, IOrderProductRepository
	{
		public OrderProductRepository(ApplicationDbContext db) : base(db)
		{
		}
	}
}
