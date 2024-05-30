using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models.DTO;
using ShoppingCart.ServiceContracts;

[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAdminService _adminService;
    private readonly ShoppingCartDbContext _context;
    
    public IMapper Mapper { get; set; }

    public UserController(IUserService userService, IAdminService adminService, ShoppingCartDbContext context)
    {
        _userService = userService;
        _adminService = adminService;
        _context = context;
        
    }

    //[HttpGet("Admin/GetAllUsers")]
    //public async Task<IActionResult> GetAllUsers([FromHeader] string username, [FromHeader] string password)
    //{
    //    if (!await _adminService.IsAdmin(username, password))
    //    {
    //        return Unauthorized();
    //    }

    //    var users = await _userService.GetAllUsers();

    //    if (users == null)
    //        return NotFound("User Not found");

    //    return Ok(users);
    //}

    [HttpGet("Admin/GetAllUsers")]
    public async Task<IActionResult> GetAllUsers([FromHeader] string username, [FromHeader] string password)
    {
        if (!await _adminService.IsAdmin(username, password))
        {
            return Unauthorized();
        }


        var users = await _context.Users.FromSqlRaw("EXECUTE GetAllUsers").ToListAsync(); //Using Stored Procedures

        if (users == null) return NotFound("User Not found");

        return Ok(users);
    }

    [HttpGet("Admin/GetUserById/{id}")]
    public async Task<IActionResult> GetUserById([FromHeader] string username, [FromHeader] string password, Guid id)
    {
        if (!await _adminService.IsAdmin(username, password))
        {
            return Unauthorized();
        }

        var user = await _userService.GetUserById(id);
        if (user == null)
            return NotFound();
        return Ok(user);
    }

    [HttpPost("User/UserRegistration")]
    public async Task<IActionResult> Register(UserDTO userDto)
    {
        var result = await _userService.Register(userDto);
        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetUserById), new { id = result.User.UserId }, result.User);
        else
            return BadRequest(result.ErrorMessage);
    }

    [HttpPut("Admin/UpdateUser/{id}")]
    public async Task<IActionResult> UpdateUser([FromHeader] string username, [FromHeader] string password, Guid id, UserDTO userDto)
    {
        if (!await _adminService.IsAdmin(username, password))
        {
            return Unauthorized();
        }

        var result = await _userService.UpdateUser(id, userDto);
        if (result.IsSuccess)
            return NoContent();
        else
            return BadRequest(result.ErrorMessage);
    }

    [HttpDelete("Admin/UserDeletion/{id}")]
    public async Task<IActionResult> DeleteUser([FromHeader] string username, [FromHeader] string password, Guid id)
    {
        if (!await _adminService.IsAdmin(username, password))
        {
            return Unauthorized();
        }

        var result = await _userService.DeleteUser(id);
        if (result.IsSuccess)
            return NoContent();
        else
            return NotFound();
    }
}
