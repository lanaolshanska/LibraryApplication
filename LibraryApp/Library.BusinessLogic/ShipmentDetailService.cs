using Library.BusinessLogic.Interfaces;
using Library.DataAccess.Repository.Interfaces;
using Library.Models;
using Library.Models.ViewModels;

namespace Library.BusinessLogic
{
	public class ShipmentDetailService : BaseService<ShipmentDetail>, IShipmentDetailService
	{
		private readonly IOrderService _orderService;
		public ShipmentDetailService(IShipmentDetailRepository repository, IOrderService orderService) : base(repository)
		{
			_orderService = orderService;
		}

		public int? UpdateShipmentDetail(ShipmentDetailVM shipmentDetailVm)
		{
			var order = _orderService.GetById(shipmentDetailVm.OrderId);
			var shipmentDetail = GetById(order.ShipmentDetailId);
			if (shipmentDetail != null)
			{
				shipmentDetail.Carrier = shipmentDetailVm.Carrier;
				shipmentDetail.TrackingNumber = shipmentDetailVm.TrackingNumber;
				shipmentDetail.ShippingDate = DateTime.Now;

				Update(shipmentDetail);
				return shipmentDetail.Id;
			}
			return null;
		}
	}
}
