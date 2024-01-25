using Library.BusinessLogic.Interfaces;
using Library.DataAccess.Repository.Interfaces;
using Library.Models;
using Library.Models.ViewModels;

namespace Library.BusinessLogic
{
	public class UserService : BaseService<ApplicationUser>, IUserService
	{
		private readonly IApplicationUserRepository _userRepository;
		public UserService(IApplicationUserRepository userRepository) : base(userRepository)
		{
			_userRepository = userRepository;
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
				CompanyName = user.Company?.Name ?? string.Empty
			}).ToList();
		}
	}
}
