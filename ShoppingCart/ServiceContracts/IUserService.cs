using ShoppingCart.Models.Domain;
using ShoppingCart.Models.DTO;

namespace ShoppingCart.ServiceContracts
{
    public interface IUserService 
    {


        Task<User> GetUserByUsername(string username);
        Task<(Guid?, string)> GetUserIdAndUsernameByCartId(Guid cartId);
        Task<Guid> GetUserIdByUsername(string username);
        Task<bool> ValidateCredentials(string username, string password);

        Task<IEnumerable<User>> GetAllUsers();

        Task<User> GetUserById(Guid id);
        Task<(bool IsSuccess, User User, string ErrorMessage)> Register(UserDTO userDto);
        Task<(bool IsSuccess, string ErrorMessage)> UpdateUser(Guid id, UserDTO userDto);
        Task<(bool IsSuccess, string ErrorMessage)> DeleteUser(Guid id);


    }

}
