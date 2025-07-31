using UserService.Web.API.Domain.Entities;

namespace UserService.Web.API.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int idUser);

    Task<User?> GetByEmailAsync(string email);

    Task<IEnumerable<User>> GetAllAsync();

    Task<User> CreateAsync(User user);

    Task<User> UpdateAsync(User user);

    Task<User> DeleteAsync(User user);
}