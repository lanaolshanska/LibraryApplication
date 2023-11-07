using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
	public class ShoppingCart
	{
		public int Id { get; set; }
		public int ProductId { get; set; }
		public required string ApplicationUserId { get; set; }
		
		[Range(1, 100, ErrorMessage = "Please enter a value between 1 and 100")]
		public int Count { get; set; }

		[ValidateNever]
		[ForeignKey("ProductId")]
		public Product Product { get; set; }

		[ValidateNever]
		[ForeignKey("ApplicationUserId")]
		public ApplicationUser ApplicationUser { get; set; }
	}
}
