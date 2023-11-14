using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
	public class ShipmentDetail
	{
		[Key]
		public int Id { get; set; }
		public DateTime ShippingDate { get; set; }
		public string? TrackingNumber { get; set; }
		public string? Carrier { get; set; }
		public int UserAddressId { get; set; }

		[ForeignKey("UserAddressId")]
		[ValidateNever]
		public UserAddress UserAddress { get; set; }

	}
}
