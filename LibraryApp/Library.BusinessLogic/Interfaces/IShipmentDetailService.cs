namespace Library.BusinessLogic.Interfaces
{
	using Library.Models;
	using Library.Models.ViewModels;

	public interface IShipmentDetailService : IBaseService<ShipmentDetail>
	{
		int? UpdateShipmentDetail(ShipmentDetailVM shipmentDetailVm);
	}
}
