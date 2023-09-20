namespace Library.Models
{
	using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class Product
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Title { get; set; }
		[Required]
		public string ISBN { get; set; }
		[Required]
		public string Author { get; set; }
		[Required]
		[Range(1, 1000)]
		public int Price { get; set; }
		public string Description { get; set; }
		[ValidateNever]
		public string ImageUrl { get; set; } = string.Empty;
		public int CategoryId { get; set; }

		[ForeignKey("CategoryId")]
		[ValidateNever]
		public Category Category { get; set; }
	}
}
