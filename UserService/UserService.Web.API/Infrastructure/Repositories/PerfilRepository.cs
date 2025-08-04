using Microsoft.EntityFrameworkCore;
using UserService.Web.API.Domain.Entities;
using UserService.Web.API.Domain.Interfaces;
using UserService.Web.API.Infrastructure.Context;

namespace UserService.Web.API.Infrastructure.Repositories;

public class PerfilRepository : IPerfilRepository
{
    private readonly ApplicationDbContext _perfilContext;

    public PerfilRepository(ApplicationDbContext perfilContext)
    {
        _perfilContext = perfilContext;
    }

    public async Task<IEnumerable<Perfil>> GetAllAsync()
    {
        return await _perfilContext.Perfil.ToListAsync();
    }

    public async Task<Perfil?> GetByIdAsync(int idPerfil)
    {
        return await _perfilContext.Perfil.Where(p => p.Id == idPerfil).FirstOrDefaultAsync();
    }
}