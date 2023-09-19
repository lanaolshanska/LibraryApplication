namespace Library.Models
{
	using System.ComponentModel.DataAnnotations;

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
	}
}
