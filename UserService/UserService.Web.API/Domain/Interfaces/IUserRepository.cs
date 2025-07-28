using UserService.Web.API.Domain.Entities;

namespace UserService.Web.API.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetById(int idUser);

    Task<IEnumerable<User>> GetAll();

    Task<User> Create(User user);

    Task<User> Update(User user);

    Task<User> Delete(User user);
}