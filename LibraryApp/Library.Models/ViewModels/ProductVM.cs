namespace Library.Models.ViewModels
{
	using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
	using Microsoft.AspNetCore.Mvc.Rendering;

	public class ProductVM
	{
		public Product Product { get; set; }
		[ValidateNever]
		public List<SelectListItem> Categories { get; set; }
	}
}
