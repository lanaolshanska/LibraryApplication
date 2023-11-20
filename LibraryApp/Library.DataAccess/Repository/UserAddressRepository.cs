using Library.DataAccess.Data;
using Library.DataAccess.Repository.Interfaces;
using Library.Models;

namespace Library.DataAccess.Repository
{
	public class UserAddressRepository : Repository<UserAddress>, IUserAddressRepository
	{
		public UserAddressRepository(ApplicationDbContext db) : base(db)
		{
		}

		public UserAddress? GetPrimaryUserAddress(string userId)
		{
			return _dbSet.Where(x => x.ApplicationUserId == userId &&
								x.IsPrimary == true)
								.FirstOrDefault();
		}

		public (bool, UserAddress?) IsUnique(UserAddress newAddress, string userId)
		{
			var existingAddress = _dbSet.Where(address =>
				address.ApplicationUserId == userId &&
				string.Equals(address.Name, newAddress.Name) &&
				string.Equals(address.PhoneNumber, newAddress.PhoneNumber) &&
				string.Equals(address.StreetAddress, newAddress.StreetAddress) &&
				string.Equals(address.City, newAddress.City) &&
				string.Equals(address.State, newAddress.State) &&
				string.Equals(address.PostalCode, newAddress.PostalCode))
				.FirstOrDefault();
			var isUnique = existingAddress == null;
			return (isUnique, existingAddress);
		}

		public void SetPrimaryAddress(int addressId, string userId)
		{
			var userAddresses = _dbSet.Where(address => address.ApplicationUserId == userId).ToList();
			userAddresses.ForEach(address => address.IsPrimary = false);
			
			var primaryAddress = userAddresses.Where(address => address.Id == addressId).First();
			primaryAddress.IsPrimary = true;
			Update(primaryAddress);
		}
	}
}
