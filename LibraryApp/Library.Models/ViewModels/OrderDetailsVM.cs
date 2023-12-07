namespace Library.Models.ViewModels
{
	public class OrderDetailsVM
	{
		public int Id { get; set; }
		public DateTime OrderDate { get; set; }
		public string OrderStatus { get; set; }
		public double Total { get; set; }
		public string Email { get; set; }
		public PaymentDetail PaymentDetail { get; set; }
		public ShipmentDetail ShipmentDetail { get; set; }
		public List<OrderProduct> Products { get; set; }
	}
}
