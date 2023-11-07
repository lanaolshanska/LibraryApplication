using Library.DataAccess.Data;
using Library.DataAccess.Repository.Interfaces;
using Library.Models;

namespace Library.DataAccess.Repository
{
	public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
	{
		public ApplicationUserRepository(ApplicationDbContext db) : base(db)
		{
		}
	}
}
