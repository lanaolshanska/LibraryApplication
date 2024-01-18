using Library.Models;

namespace Library.DataAccess.Repository.Interfaces
{
	public interface IOrderRepository : IRepository<Order>
	{
		Order? GetDetailsById(int id);
		IEnumerable<Order> GetOrdersByCustomerId(string customerId);
	}
}
