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

    public async Task<User> CreateAsync(User user)
    {
        _userContext.Add(user);
        await _userContext.SaveChangesAsync();

        return await _userContext.Users.Include(u => u.Perfil).FirstAsync(u => u.Id == user.Id);
    }

    public async Task<User> DeleteAsync(User user)
    {
        _userContext.Update(user);
        await _userContext.SaveChangesAsync();
        return user;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _userContext.Users.Include(p => p.Perfil).ToListAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _userContext.Users.Include(p => p.Perfil).Where(u => u.Email.ToLower().Equals(email.ToLower())).FirstOrDefaultAsync();
    }

    public async Task<User?> GetByIdAsync(int idUser)
    {
        return await _userContext.Users.Include(p => p.Perfil).Where(u => u.Id == idUser).FirstOrDefaultAsync();
    }

    public async Task<User> UpdateAsync(User user)
    {
        _userContext.Update(user);
        await _userContext.SaveChangesAsync();
        return user;
    }
}
