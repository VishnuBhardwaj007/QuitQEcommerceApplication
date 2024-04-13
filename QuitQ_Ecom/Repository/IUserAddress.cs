using QuitQ_Ecom.DTOs;

namespace QuitQ_Ecom.Repository
{
    public interface IUserAddress
    {
        Task<UserAddressDTO> GetActiveUserAddressByUserId(int userId);
        Task<UserAddressDTO> AddUserAddress(UserAddressDTO userAddressDTO);
        Task<bool> DeleteUserAddress(int userAddressId);
        Task<List<UserAddressDTO>> GetUserAddressesByUserId(int userId);
        Task<UserAddressDTO> UpdateUserAddress(int userAddressId, UserAddressDTO userAddressDTO);
    }
}
