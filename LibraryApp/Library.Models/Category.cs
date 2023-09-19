namespace Library.Models
{
	using System.ComponentModel.DataAnnotations;

	public class Category
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[MaxLength(30)]
		public string Name { get; set; }
	}
}
