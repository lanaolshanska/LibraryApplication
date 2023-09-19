namespace Library.DataAccess.Repository
{
    using Library.DataAccess.Data;
    using Library.DataAccess.Repository.Interfaces;
    using Library.Models;

    public class CategoryRepository : Repository<Category>, ICategoryRepository
	{
		public CategoryRepository(ApplicationDbContext db) : base(db)
		{
		}
	}
}
