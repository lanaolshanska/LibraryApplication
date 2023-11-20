using Library.Models;

namespace Library.DataAccess.Repository.Interfaces
{
	public interface IUserAddressRepository : IRepository<UserAddress>
	{
		UserAddress? GetPrimaryUserAddress(string userId);
		(bool, UserAddress?) IsUnique(UserAddress newAddress, string userId);
		void SetPrimaryAddress(int addressId, string userId);
	}
}
