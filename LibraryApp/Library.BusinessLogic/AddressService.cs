using Library.BusinessLogic.Interfaces;
using Library.DataAccess.Repository.Interfaces;
using Library.Models;

namespace Library.BusinessLogic
{
    public class AddressService : BaseService<UserAddress>, IAddressService
    {
        private readonly IUserAddressRepository _addressRepository;

        public AddressService(IUserAddressRepository addressRepository) : base(addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public UserAddress? GetPrimaryUserAddress(string userId)
        {
            return _addressRepository.GetPrimaryUserAddress(userId);
        }

        public int CreateOrUpdateAddress(UserAddress newAddress, string userId)
        {
            int primaryAddressId;
            var (isAddressUnique, existingAddress) = _addressRepository.IsUnique(newAddress, userId);
            if (isAddressUnique)
            {
                newAddress.ApplicationUserId = userId;
                Create(newAddress);
                primaryAddressId = newAddress.Id;
            }
            else
            {
                primaryAddressId = existingAddress.Id;
            }
            _addressRepository.SetPrimaryAddress(primaryAddressId, userId);
            return primaryAddressId;
        }
    }
}
