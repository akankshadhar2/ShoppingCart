
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.ServiceContracts;

namespace ShoppingCart.Services
{
    public class AdminService : IAdminService
    {
        private readonly ShoppingCartDbContext _context;

        public AdminService(ShoppingCartDbContext context)
        {
            _context = context;
        }
       
        public async Task<bool> IsAdmin(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);
            return user != null && user.RoleId == "ADM";
        }


    }

}



