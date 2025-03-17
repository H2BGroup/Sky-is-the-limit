namespace reservation_service.Models.DTO;

public class GetUserReponse
{
    required public string Id { get; set; }
    public string? Login { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
}

public class GetUsersResponse
{
    public class SimpleUser
    {
        required public string Id { get; set; }
        public string? Login { get; set; }
    }
    public IEnumerable<SimpleUser> Users { get; set; } = []; 
}

public class PutUserRequest
{
    public string? Login { get; set; }
    public string? Password { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
}

public class UserLoginForm
{
    required public string Login { get; set; }
    required public string Password { get; set; }
}