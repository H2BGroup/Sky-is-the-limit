using Microsoft.AspNetCore.Mvc;
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
    public IEnumerable<User> GetUsers()
    {
        return _userService.GetUsers();
    }

    [HttpGet("{id}")]
    public User? GetUser(string id)
    {
        return _userService.GetUser(id);
    }

    [HttpGet("login/{login}")]
    public User? GetUserByLogin(string login)
    {
        return _userService.GetUserByLogin(login);
    }
    
    [HttpPut]
    public IActionResult Create(User user)
    {
        _userService.Create(user);
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        _userService.Delete(id);
        return NoContent();
    }
}