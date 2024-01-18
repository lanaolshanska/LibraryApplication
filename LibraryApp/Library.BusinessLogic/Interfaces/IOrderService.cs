using Library.Models;
using Library.Models.ViewModels;

namespace Library.BusinessLogic.Interfaces
{
    public interface IOrderService : IBaseService<Order>
    {
        new IEnumerable<OrderVM> GetAll();
        Order CreateOrder(ApplicationUser user, UserAddress address, List<ShoppingCart> shoppingCarts);
        OrderDetailsVM? GetDetailsById(int id);
        void UpdateStatus(int id, string status);
        double CalculateDiscountOrderTotal(double orderTotal, double discount);
        IEnumerable<Order> GetOrdersByCustomerId(string customerId);

	}
}
