using Library.Models;

namespace Library.DataAccess.Repository.Interfaces
{
	public interface IApplicationUserRepository : IRepository<ApplicationUser>
	{
		public ApplicationUser GetById(string id);
	}
}
