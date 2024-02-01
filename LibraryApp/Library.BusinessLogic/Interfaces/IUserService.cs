using Library.Models;
using Library.Models.ViewModels;

namespace Library.BusinessLogic.Interfaces
{
	public interface IUserService : IBaseService<ApplicationUser>
	{
		ApplicationUser? GetById(string id);
		RoleManagementVM? GetRoleManagementDetails(string userId);
		new IEnumerable<UserVM> GetAll();
	}
}
