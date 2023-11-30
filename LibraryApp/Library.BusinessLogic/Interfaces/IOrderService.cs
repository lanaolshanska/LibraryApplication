using Library.Models;

namespace Library.BusinessLogic.Interfaces
{
    public interface IOrderService : IBaseService<Order>
    {
        Order CreateOrder(ApplicationUser user, UserAddress address, List<ShoppingCart> shoppingCarts);
    }
}
