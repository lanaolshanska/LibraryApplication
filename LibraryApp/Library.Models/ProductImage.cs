﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
	public class ProductImage
	{
		[Key]
		public int Id { get; set; }
		public string ImageUrl { get; set; }
		public int ProductId { get; set; }
		public bool IsCover { get; set; }
	}
}
