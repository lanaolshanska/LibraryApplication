using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
	public class ApplicationUser : IdentityUser
	{
		public int? CompanyId { get; set; }

		[ForeignKey("CompanyId")]
		[ValidateNever]
		public Company? Company { get; set; }

		public int? AddressId { get; set; }

		[ForeignKey("AddressId")]
		[ValidateNever]
		public UserAddress? Address { get; set; }
	}
}
