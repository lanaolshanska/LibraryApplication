﻿using Library.Models;

namespace Library.BusinessLogic.Interfaces
{
	public interface IProductService : IBaseService<Product>
	{
		void Delete(Product product);
	}
}
