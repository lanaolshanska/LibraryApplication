namespace Library.DataAccess.Repository
{
	using Library.DataAccess.Data;
	using Library.DataAccess.Repository.Interfaces;
	using Library.Models;
	using Microsoft.EntityFrameworkCore;

	public class ProductRepository : Repository<Product>, IProductRepository
	{
		public ProductRepository(ApplicationDbContext db) : base(db) { }

		public override IEnumerable<Product> GetAll()
		{
			return _dbSet
				.Include(t => t.Category)
				.ToList();
		}
	}
}
