using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models.DTO;

namespace ShoppingCart.ServiceContracts
{
    public interface IAdminService
    {
        
        Task<bool> IsAdmin(string username, string password);
        
    }

}
