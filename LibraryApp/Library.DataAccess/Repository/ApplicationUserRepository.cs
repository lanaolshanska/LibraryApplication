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

		public override IEnumerable<ApplicationUser> GetAll()
		{
			var users = _dbSet.Include(t => t.Addresses)
						.Include(t => t.Company).ToList();
			
			var roles = _db.Roles;
			var userRoles = _db.UserRoles;

			foreach ( var user in users )
			{
				var roleId = userRoles.First(x => x.UserId == user.Id).RoleId;
				user.Role = roles.Find(roleId).Name;
			}
			return users;
		}

		public ApplicationUser? GetById(string id)
		{
			return _dbSet.Include(t => t.Addresses)
						.Where(x => x.Id.Equals(id))
						.FirstOrDefault();
		}
	}
}
