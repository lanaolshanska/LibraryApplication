namespace Library.DataAccess.Repository
{
	using Library.DataAccess.Data;
	using Library.DataAccess.Repository.Interfaces;
	using Library.Models;

	public class ProductRepository : Repository<Product>, IProductRepository
	{
		public ProductRepository(ApplicationDbContext db) : base(db)
		{

		}
	}
}
