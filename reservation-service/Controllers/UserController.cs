using Microsoft.AspNetCore.Mvc;
using reservation_service.Models.DTO;
using reservation_service.Models;
using reservation_service.Services;

namespace reservation_service.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public GetUsersResponse GetUsers()
    {
        return UserDTOMapper.UsersToResponse(_userService.GetUsers());
    }

    [HttpGet("{id}")]
    public ActionResult<GetUserReponse> GetUser(string id)
    {
        User? user = _userService.GetUser(id);
        if (user == null)
        {
            return NotFound();
        }
        return UserDTOMapper.UserToResponse(user);
    }

    [HttpPost("login")]
    public ActionResult<GetUserReponse> Login([FromForm] UserLoginForm userForm)
    {
        User? user = _userService.GetUserByLogin(userForm.Login);
        if (user == null)
        {
            return Unauthorized();
        }
        if (user.Password != userForm.Password)
        {
            return Unauthorized();
        }
        return UserDTOMapper.UserToResponse(user);
    }
    
    [HttpPut("{id}")]
    public IActionResult Create(string id, PutUserRequest user)
    {
        try
        {
            _userService.Create(UserDTOMapper.RequestToUser(id, user));
        } 
        catch(ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        return CreatedAtAction(nameof(GetUser), new { id = id }, user);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        _userService.Delete(id);
        return NoContent();
    }
}