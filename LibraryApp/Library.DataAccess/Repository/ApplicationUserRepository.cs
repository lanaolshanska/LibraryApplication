using Library.DataAccess.Data;
using Library.DataAccess.Repository.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.Repository
{
	public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
	{
		private readonly ICompanyRepository _companyRepository;
		public ApplicationUserRepository(ApplicationDbContext db, ICompanyRepository companyRepository) : base(db)
		{
			_companyRepository = companyRepository;
		}

		public override IEnumerable<ApplicationUser> GetAll()
		{
			var users = _dbSet.Include(t => t.Addresses)
						.Include(t => t.Company).ToList();

			var roles = _db.Roles;
			var userRoles = _db.UserRoles;

			foreach (var user in users)
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

		public (ApplicationUser?, List<string>, IEnumerable<Company>) GetRoleManagementDetails(string userId)
		{
			var user = _dbSet.Include(t => t.Company)
							.Where(x => x.Id.Equals(userId))
							.FirstOrDefault();
			if (user != null)
			{
				user.Role = GetUserRole(user.Id);
				var companies = _companyRepository.GetAll();
				var roles = _db.Roles.Select(x => x.Name).ToList();

				return (user, roles, companies);
			}
			return (null, null, null);
		}

		private string GetUserRole(string userId)
		{
			var roleId = _db.UserRoles.First(x => x.UserId == userId).RoleId;
			return _db.Roles.Find(roleId).Name;
		}
	}
}
