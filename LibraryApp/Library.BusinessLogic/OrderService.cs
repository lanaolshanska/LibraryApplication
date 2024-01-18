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

		public IEnumerable<Order> GetOrdersByCustomerId(string customerId)
		{
			return _orderRepository.GetOrdersByCustomerId(customerId);
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

			var paymentDetail = new PaymentDetail { Status = PaymentStatus.Pending };
			_paymentService.Create(paymentDetail);

			var order = new Order
			{
				Date = DateTime.Now,
				Total = shoppingCarts.Sum(x => x.Count * x.Product.Price),
				Status = OrderStatus.Pending,
				ApplicationUserId = user.Id,
				ShipmentDetailId = shipmentDetail.Id,
				PaymentDetailId = paymentDetail.Id
			};

			if (user.CompanyId.HasValue && Discount.CompanyUser != 0)
			{
				order.Total = CalculateDiscountOrderTotal(order.Total, Discount.CompanyUser);
			}

			Create(order);

			shoppingCarts.ForEach(shoppingCart =>
			{
				var product = new OrderProduct
				{
					OrderId = order.Id,
					ProductId = shoppingCart.ProductId,
					Count = shoppingCart.Count,
					Price = shoppingCart.Product.Price,
					Discount = user.CompanyId.HasValue ? Discount.CompanyUser : 0
				};
				_orderProductRepository.Create(product);
			});

			return order;
		}

		public void UpdateStatus(int id, string status)
		{
			var order = _orderRepository.GetById(id);
			order.Status = status;
			_orderRepository.Update(order);
		}

		public double CalculateDiscountOrderTotal(double orderTotal, double discount)
		{
			return orderTotal - orderTotal * discount;
		}
	}
}
