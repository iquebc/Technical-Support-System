using UserService.Web.API.Application.DTOs;

namespace UserService.Web.API.Application.Interfaces;

public interface IUserService
{
    Task<UserResponseDTO?> GetByIdAsync(int idUser);

    Task<UserResponseDTO?> GetByEmailAsync(string email);

    Task<IEnumerable<UserResponseDTO>> GetAllAsync();

    Task<UserResponseDTO> CreateAsync(UserRequestDTO userRequest);

    Task<UserResponseDTO> UpdateRegisterDataAsync(UserUpdateRegisterDTO userRequest);

    Task<UserResponseDTO> UpdateEmailAsync(UserUpdateEmailDTO userRequest);

    Task<UserResponseDTO> UpdatePasswordAsync(UserUpdatePasswordDTO userRequest);

    Task<UserResponseDTO> UpdatePerfilAsync(UserUpdatePerfilDTO userRequest);

    Task<bool> UserAlreadyRegistered(string email);

    Task<UserResponseDTO> DeleteAsync(int idUser);
}