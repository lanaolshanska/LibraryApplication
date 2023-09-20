namespace Library.DataAccess.Repository.Interfaces
{
    using Library.Models;
	using Microsoft.AspNetCore.Mvc.Rendering;

	public interface ICategoryRepository : IRepository<Category>
    {
		List<SelectListItem> GetCategoriesList();
    }
}
