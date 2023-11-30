using Library.BusinessLogic.Interfaces;
using Library.DataAccess.Repository.Interfaces;
using Library.Models;
using Library.Utility;

namespace Library.BusinessLogic
{
    public class OrderService : BaseService<Order>, IOrderService
    {
        private readonly IOrderProductRepository _orderProductRepository;
        private readonly IShipmentDetailRepository _shipmentRepository;
        private readonly IPaymentService _paymentService;
        private readonly IAddressService _addressService;

        public OrderService(IOrderRepository orderRepository,
            IOrderProductRepository orderProductRepository,
            IShipmentDetailRepository shipmentRepository,
            IPaymentService paymentService,
            IAddressService addressService
            ) : base(orderRepository)
        {
            _orderProductRepository = orderProductRepository;
            _shipmentRepository = shipmentRepository;
            _paymentService = paymentService;
            _addressService = addressService;
        }

        public Order CreateOrder(ApplicationUser user, UserAddress address, List<ShoppingCart> shoppingCarts)
        {
            var addressId = _addressService.CreateOrUpdateAddress(address, user.Id);

            var shipmentDetail = new ShipmentDetail { UserAddressId = addressId };
            _shipmentRepository.Create(shipmentDetail);

            var paymentDetail = new PaymentDetail { Status = user.CompanyId.HasValue ? PaymentStatus.Delayed : PaymentStatus.Pending };
            _paymentService.Create(paymentDetail);

            var order = new Order
            {
                Date = DateTime.Now,
                Total = shoppingCarts.Sum(x => x.Count * x.Product.Price),
                Status = user.CompanyId.HasValue ? OrderStatus.Approved : OrderStatus.Pending,
                ApplicationUserId = user.Id,
                ShipmentDetailId = shipmentDetail.Id,
                PaymentDetailId = paymentDetail.Id
            };

            Create(order);

            shoppingCarts.ForEach(shoppingCart =>
            {
                var product = new OrderProduct
                {
                    OrderId = order.Id,
                    ProductId = shoppingCart.ProductId,
                    Count = shoppingCart.Count,
                    Price = shoppingCart.Product.Price,
                };
                _orderProductRepository.Create(product);
            });

            return order;
        }
    }
}
