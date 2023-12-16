using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
	public class OrderProduct
	{
		[Key]
		public int Id { get; set; }
		public int OrderId { get; set; }
		public int ProductId { get; set; }
		public int Count { get; set; }
		public double Price { get; set; }
		public double Discount { get; set; }

		[ForeignKey("ProductId")]
		[ValidateNever]
		public Product Product { get; set; }
	}
}
