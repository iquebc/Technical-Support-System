using UserService.Web.API.Domain.Entities;

namespace UserService.Web.API.Domain.Interfaces;

public interface IPerfilRepository
{
    Task<Perfil?> GetByIdAsync(int idPerfil);

    Task<IEnumerable<Perfil>> GetAllAsync();
}