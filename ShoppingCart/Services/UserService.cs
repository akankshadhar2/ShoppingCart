using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models.Domain;
using ShoppingCart.Models.DTO;
using ShoppingCart.ServiceContracts;
using System.Text.RegularExpressions;

public class UserService : IUserService
{
    private readonly ShoppingCartDbContext _context;
    private readonly IMapper _mapper;

    public UserService(ShoppingCartDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<User> GetUserByUsername(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
    }

    public async Task<(Guid?, string)> GetUserIdAndUsernameByCartId(Guid cartId)
    {
        var cart = await _context.Carts
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.CartId == cartId);

        if (cart == null)
        {
            return (null, null);
        }

        return (cart.UserId, cart.User?.UserName);
    }

    public async Task<Guid> GetUserIdByUsername(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        return user?.UserId ?? Guid.Empty;
    }
    public async Task<bool> ValidateCredentials(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

        // Check if user exists
        if (user == null)
        {
            return false;
        }

        else
        {
            return true;
        }
    }
    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetUserById(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<(bool IsSuccess, User User, string ErrorMessage)> Register(UserDTO userDto)
    {
        if (string.IsNullOrEmpty(userDto.UserName) || string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Password))
        {
            return (false, null, "Please provide all required fields: Username, Email, Password");
        }

        if (string.IsNullOrEmpty(userDto.UserName))
        {
            return (false, null, "Username cannot be empty");
        }

        if (userDto.UserName.Length < 4 || userDto.UserName.Length > 20)
        {
            return (false, null, "Username must be between 4 and 20 characters long");
        }

        const string usernameRegex = @"^[a-zA-Z0-9_]+$";
        if (!Regex.IsMatch(userDto.UserName, usernameRegex))
        {
            return (false, null, "Username can only contain letters, numbers, and underscores");
        }

        string[] disallowedUsernames = { "admin", "administrator", "root" };
        if (disallowedUsernames.Contains(userDto.UserName.ToLower()))
        {
            return (false, null, "Username is not allowed");
        }

        if (await _context.Users.AnyAsync(u => u.UserName.ToLower() == userDto.UserName.ToLower()))
        {
            return (false, null, "Username already exists");
        }

        if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
        {
            return (false, null, "Email already exists");
        }

        const string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        if (!Regex.IsMatch(userDto.Email, emailRegex))
        {
            return (false, null, "Invalid email format");
        }

        User user = _mapper.Map<User>(userDto); //Using AutoMapper
        user.RoleId = "USER";
        user.UserId = Guid.NewGuid();

     
        //User user = new User
        //{
        //    UserId = Guid.NewGuid(),
        //    UserName = userDto.UserName,
        //    Email = userDto.Email,
        //    Password = userDto.Password, // Assuming you're handling password hashing securely
        //    RoleId = "USER"
        //};

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return (true, user, null);
    }

    public async Task<(bool IsSuccess, string ErrorMessage)> UpdateUser(Guid id, UserDTO userDto)
    {
        User user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return (false, "User not found");
        }

        

        user.UserName = userDto.UserName;
        user.Email = userDto.Email;

        if (!string.IsNullOrEmpty(userDto.Password))
        {
            user.Password = userDto.Password;
        }

        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return (true, null);
    }

    public async Task<(bool IsSuccess, string ErrorMessage)> DeleteUser(Guid id)
    {
        User user = await _context.Users.FindAsync(id);
        if (user == null)
            return (false, "User not found");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return (true, null);
    }
}



