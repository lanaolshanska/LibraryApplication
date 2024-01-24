using Library.DataAccess.Data;
using Library.Models;
using Library.Utility.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.DBInitializer
{
	public class DbInitializer : IDbInitializer
	{
		private readonly ApplicationDbContext _db;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<IdentityUser> _userManager;

		public DbInitializer(ApplicationDbContext db,
			RoleManager<IdentityRole> roleManager,
			UserManager<IdentityUser> userManager)
		{
			_db = db;
			_roleManager = roleManager;
			_userManager = userManager;
		}

		public async Task Initialize()
		{
			try
			{
				if(_db.Database.GetPendingMigrations().Count() > 0)
				{
					await _db.Database.MigrateAsync();
				}
			}
			catch(Exception ex)
			{

			}

			if (!(await _roleManager.RoleExistsAsync(Role.Admin)))
			{
				await SetRoles();
				await CreateDefaultAdminUser();
			}
		}

		private async Task SetRoles()
		{
			await _roleManager.CreateAsync(new IdentityRole(Role.Admin));
			await _roleManager.CreateAsync(new IdentityRole(Role.Company));
			await _roleManager.CreateAsync(new IdentityRole(Role.Customer));
		}

		private async Task CreateDefaultAdminUser()
		{
			var password = "Admin123!";
			var adminUser = new ApplicationUser
			{
				UserName = "admin@dotnet.com",
				Email = "admin@dotnet.com"
			};

			await _userManager.CreateAsync(adminUser, password);
			await _userManager.AddToRoleAsync(adminUser, Role.Admin);
		}
	}
}
