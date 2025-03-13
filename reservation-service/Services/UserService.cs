using reservation_service.Models;

namespace reservation_service.Services;

public class UserService : IUserService
{
    private readonly ReservationContext _context;

    public UserService(ReservationContext context)
    {
        _context = context;
    }

    public void Create(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void Delete(string id)
    {
        var user = _context.Users.Find(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }

    public User? GetUser(string id)
    {
        return _context.Users.Find(id);
    }

    public User? GetUserByLogin(string login)
    {
        return _context.Users.FirstOrDefault(u => u.Login == login);
    }

    public IEnumerable<User> GetUsers()
    {
        return _context.Users.ToList();
    }
}