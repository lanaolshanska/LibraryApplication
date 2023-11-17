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
	}
}
