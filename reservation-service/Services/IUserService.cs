using reservation_service.Models;

namespace reservation_service.Services;

public interface IUserService
{
    public IEnumerable<User> GetUsers();
    public User? GetUser(string id);
    public User? GetUserByLogin(string login);
    public void Create(User user);
    public void Delete(string id);
}
