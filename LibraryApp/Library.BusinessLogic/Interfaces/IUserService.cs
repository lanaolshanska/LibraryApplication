using Library.Models;
using Library.Models.ViewModels;

namespace Library.BusinessLogic.Interfaces
{
	public interface IUserService : IBaseService<ApplicationUser>
	{
		new IEnumerable<UserVM> GetAll();
	}
}
