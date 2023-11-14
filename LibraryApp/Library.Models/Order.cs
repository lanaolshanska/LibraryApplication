using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
	public class Order
	{
		[Key]
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public double Total { get; set; }
		public string? Status { get; set; }
		public string ApplicationUserId { get; set; }
		public int ShipmentDetailId { get; set; }
		public int PaymentDetailId { get; set; }

		[ForeignKey("ShipmentDetailId")]
		[ValidateNever]
		public ShipmentDetail ShipmentDetail { get; set; }

		[ForeignKey("PaymentDetailId")]
		[ValidateNever]
		public PaymentDetail PaymentDetail { get; set; }

		[ForeignKey("ApplicationUserId")]
		[ValidateNever]
		public ApplicationUser ApplicationUser { get; set; }
	}
}
