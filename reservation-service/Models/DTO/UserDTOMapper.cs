namespace reservation_service.Models.DTO;

public static class UserDTOMapper
{
    public static GetUserReponse UserToResponse(User user)
    {
        return new GetUserReponse
        {
            Id = user.Id,
            Login = user.Login,
            Name = user.Name,
            Surname = user.Surname
        };
    }

    public static GetUsersResponse UsersToResponse(IEnumerable<User> users)
    {
        return new GetUsersResponse
        {
            Users = users.Select(u => new GetUsersResponse.SimpleUser
            {
                Id = u.Id,
                Login = u.Login
            })
        };
    }

    public static User RequestToUser(string id, PutUserRequest request)
    {
        return new User
        {
            Id = id,
            Login = request.Login,
            Password = request.Password,
            Name = request.Name,
            Surname = request.Surname
        };
    }
}
