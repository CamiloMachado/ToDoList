using ToDo_GPT.Models;

namespace ToDo_GPT.Repositories;

public interface IUserRepository
{
    Task<User> GetUserAsync(string email, string password);

    Task<User> CreateUserAsync(User user);

    Task<User> UpdateUserAsync(int id, User user);

    Task<User> DeleteUserAsync(int id);
}