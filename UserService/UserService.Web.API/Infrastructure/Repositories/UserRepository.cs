using Microsoft.EntityFrameworkCore;
using UserService.Web.API.Domain.Entities;
using UserService.Web.API.Domain.Interfaces;
using UserService.Web.API.Infrastructure.Context;

namespace UserService.Web.API.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _userContext;

    public UserRepository(ApplicationDbContext userContext)
    {
        _userContext = userContext;
    }

    public async Task<User> Create(User user)
    {
        _userContext.Add(user);
        await _userContext.SaveChangesAsync();
        return user;
    }

    public async Task<User> Delete(User user)
    {
        _userContext.Update(user);
        await _userContext.SaveChangesAsync();
        return user;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _userContext.Users.ToListAsync();
    }

    public async Task<User?> GetById(int idUser)
    {
        return await _userContext.Users.Where(u => u.Id == idUser).FirstOrDefaultAsync();
    }

    public async Task<User> Update(User user)
    {
        _userContext.Update(user);
        await _userContext.SaveChangesAsync();
        return user;
    }
}
