using Library.BusinessLogic.Interfaces;
using Library.DataAccess.Repository.Interfaces;
using Library.Models;
using Library.Models.ViewModels;
using Library.Utility.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.BusinessLogic
{
	public class UserService : BaseService<ApplicationUser>, IUserService
	{
		private readonly IApplicationUserRepository _userRepository;
		private readonly UserManager<IdentityUser> _userManager;

		public UserService(IApplicationUserRepository userRepository, UserManager<IdentityUser> userManager) : base(userRepository)
		{
			_userRepository = userRepository;
			_userManager = userManager;

		}

		public new IEnumerable<UserVM> GetAll()
		{
			var users = _userRepository.GetAll();
			return users.Select(user => new UserVM
			{
				Id = user.Id,
				Role = user.Role,
				Email = user.Email,
				Name = user.Addresses?.Where(x => x.IsPrimary == true).FirstOrDefault()?.Name ?? string.Empty,
				PhoneNumber = user.Addresses?.Where(x => x.IsPrimary == true).FirstOrDefault()?.PhoneNumber ?? string.Empty,
				CompanyName = user.Company?.Name ?? string.Empty,
				LockoutEnd = user.LockoutEnd
			}).ToList();
		}

		public ApplicationUser? GetById(string id)
		{
			return _userRepository.GetById(id);
		}

		public RoleManagementVM? GetRoleManagementDetails(string userId)
		{
			var (user, roles, companies) = _userRepository.GetRoleManagementDetails(userId);
			if (user != null)
			{
				var roleManagementVm = new RoleManagementVM
				{
					UserId = userId,
					Email = user.Email,
					Role = user.Role,
					CompanyId = user.Company?.Id,
					CompanyList = companies.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList(),
					RoleList = roles.Select(x => new SelectListItem(x, x)).ToList()
				};
				return roleManagementVm;
			}
			return null;
		}

		public async Task<string?> UpdateRole(RoleManagementVM roleManagementDetails)
		{
			var identityUser = await _userManager.FindByIdAsync(roleManagementDetails.UserId);
			if (identityUser != null)
			{
				var user = GetById(identityUser.Id);
				var previousRole = _userRepository.GetUserRole(user.Id);
				if (roleManagementDetails.Role == Role.Company)
				{
					user.CompanyId = roleManagementDetails.CompanyId;
					Update(user);
				}
				else
				{
					if (previousRole == Role.Company) user.CompanyId = null;
					Update(user);
				}
				
				await _userManager.RemoveFromRoleAsync(identityUser, previousRole);
				await _userManager.AddToRoleAsync(identityUser, roleManagementDetails.Role);
				return user.Id;
			}
			return null;
		}
	}
}
