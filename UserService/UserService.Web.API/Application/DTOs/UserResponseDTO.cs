namespace UserService.Web.API.Application.DTOs;

public class UserResponseDTO
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Sobrenome { get; set; }
    public string Email { get; set; }
    public bool Ativo { get; set; }
    public PerfilDTO Perfil { get; set; }
}