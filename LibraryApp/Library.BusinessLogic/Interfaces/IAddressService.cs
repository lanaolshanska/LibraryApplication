using Library.Models;

namespace Library.BusinessLogic.Interfaces
{
    public interface IAddressService : IBaseService<UserAddress>
    {
        UserAddress? GetPrimaryUserAddress(string userId);
        int CreateOrUpdateAddress(UserAddress newAddress, string userId);
    }
}
