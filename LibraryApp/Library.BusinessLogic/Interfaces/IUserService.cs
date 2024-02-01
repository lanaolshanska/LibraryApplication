using Library.Models;
using Library.Models.ViewModels;

namespace Library.BusinessLogic.Interfaces
{
	public interface IUserService : IBaseService<ApplicationUser>
	{
		ApplicationUser? GetById(string id);
		new IEnumerable<UserVM> GetAll();
		RoleManagementVM? GetRoleManagementDetails(string userId);
		Task<string?> UpdateRole(RoleManagementVM roleManagementDetails);
	}
}
