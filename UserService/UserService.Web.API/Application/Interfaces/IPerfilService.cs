using UserService.Web.API.Application.DTOs;

namespace UserService.Web.API.Application.Interfaces;

public interface IPerfilService
{
    Task<PerfilDTO?> GetById(int idPerfil);

    Task<IEnumerable<PerfilDTO>> GetAll();
}
