using Library.BusinessLogic.Interfaces;
using Library.DataAccess.Repository.Interfaces;
using Library.Models;
using Library.Models.ViewModels;
using Library.Utility;

namespace Library.BusinessLogic
{
    public class OrderService : BaseService<Order>, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
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
            _orderRepository = orderRepository;
            _orderProductRepository = orderProductRepository;
            _shipmentRepository = shipmentRepository;
            _paymentService = paymentService;
            _addressService = addressService;
        }

        public new IEnumerable<OrderVM> GetAll()
        {
            var orders = _orderRepository.GetAll();
            return orders.Select(order => new OrderVM
            {
                Id = order.Id,
                UserName = order.ShipmentDetail.UserAddress.Name,
                PhoneNumber = order.ShipmentDetail.UserAddress.PhoneNumber,
                Email = order.ApplicationUser.Email,
                Status = order.Status,
                Total = order.Total
            }).ToList();
        }

        public OrderDetailsVM? GetDetailsById(int id)
        {
            var order = _orderRepository.GetDetailsById(id);
            if (order != null)
            {
                var orderDetails = new OrderDetailsVM
                {
                    Id = order.Id,
                    OrderDate = order.Date,
                    OrderStatus = order.Status,
                    Total = order.Total,
                    PaymentDetail = order.PaymentDetail,
                    Email = order.ApplicationUser.Email,
                    ShipmentDetail = order.ShipmentDetail,
                    Products = order.Products,
                };
                return orderDetails;
            }
            return null;
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
