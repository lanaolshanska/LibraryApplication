﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
	public class UserAddress
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string PhoneNumber { get; set; }
		[Required]
		public string StreetAddress { get; set; }
		[Required]
		public string City { get; set; }
		[Required]
		public string State { get; set; }
		[Required]
		public string PostalCode { get; set; }
		public bool IsPrimary { get; set; }
		[ValidateNever]
		public string ApplicationUserId { get; set; }
		public ApplicationUser? ApplicationUser { get; set; }

	}
}
