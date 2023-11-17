using Library.Models;

namespace Library.DataAccess.Repository.Interfaces
{
	public interface IUserAddressRepository : IRepository<UserAddress>
	{
		UserAddress? GetPrimaryUserAddress(string userId);
	}
}
