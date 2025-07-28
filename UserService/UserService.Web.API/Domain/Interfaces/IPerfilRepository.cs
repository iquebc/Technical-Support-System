using UserService.Web.API.Domain.Entities;

namespace UserService.Web.API.Domain.Interfaces;

public interface IPerfilRepository
{
    Task<Perfil?> GetById(int idPerfil);

    Task<IEnumerable<Perfil>> GetAll();
}