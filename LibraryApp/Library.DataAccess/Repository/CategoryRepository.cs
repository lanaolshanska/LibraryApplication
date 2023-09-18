namespace Library.DataAccess.Repository
{
	using Library.DataAccess.Data;
	using Library.Models;

	public class CategoryRepository : Repository<Category>, ICategoryRepository
	{
		public CategoryRepository(ApplicationDbContext db) : base(db)
		{
		}
	}
}
