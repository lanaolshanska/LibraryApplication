using Library.DataAccess.Data;
using Library.DataAccess.Repository.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repository
{
	public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
	{
		public ApplicationUserRepository(ApplicationDbContext db) : base(db)
		{
		}

		public ApplicationUser? GetById(string id)
		{
			return _dbSet.Include(t => t.Addresses)
						.Where(x => x.Id.Equals(id))
						.FirstOrDefault();
		}
	}
}
