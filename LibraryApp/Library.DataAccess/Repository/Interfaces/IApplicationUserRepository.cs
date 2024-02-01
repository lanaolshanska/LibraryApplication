using Library.Models;

namespace Library.DataAccess.Repository.Interfaces
{
	public interface IApplicationUserRepository : IRepository<ApplicationUser>
	{
		ApplicationUser GetById(string id);
		string GetUserRole(string userId);
		(ApplicationUser?, List<string>, IEnumerable<Company>) GetRoleManagementDetails(string userId);
	}
}
