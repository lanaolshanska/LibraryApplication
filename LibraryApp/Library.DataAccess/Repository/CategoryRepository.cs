namespace Library.DataAccess.Repository
{
	using Library.DataAccess.Data;
	using Library.DataAccess.Repository.Interfaces;
	using Library.Models;
	using Microsoft.AspNetCore.Mvc.Rendering;

	public class CategoryRepository : Repository<Category>, ICategoryRepository
	{
		public CategoryRepository(ApplicationDbContext db) : base(db)
		{
		}

		public List<SelectListItem> GetCategoriesList()
		{
			var categories = GetAll().Select(x =>
				new SelectListItem
				{
					Text = x.Name,
					Value = x.Id.ToString(),
				});
			return categories.ToList();
		}
	}
}
