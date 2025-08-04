using AutoMapper;
using UserService.Web.API.Application.DTOs;
using UserService.Web.API.Application.Interfaces;
using UserService.Web.API.Domain.Entities;
using UserService.Web.API.Domain.Interfaces;
using UserService.Web.API.Domain.Validation;

namespace UserService.Web.API.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPerfilRepository _perfilRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper, IPerfilRepository perfilRepository)
    {
        _userRepository = userRepository;
        _perfilRepository = perfilRepository;
        _mapper = mapper;
    }

    public async Task<UserResponseDTO> CreateAsync(UserRequestDTO userRequest)
    {
        DomainValidationException.When(await UserAlreadyRegistered(userRequest.Email), "E-mail já Cadastrado");

        User user = _mapper.Map<User>(userRequest);
        user.AlterarPerfil(1);
        user.Ativar();

        User createdUser = await _userRepository.CreateAsync(user);
        return _mapper.Map<UserResponseDTO>(createdUser);
    }

    public async Task<UserResponseDTO> DeleteAsync(int idUser)
    {
        User user = await GetExistingUser(idUser);
        user.Inativar();

        User userDeleted = await _userRepository.DeleteAsync(user);
        return _mapper.Map<UserResponseDTO>(userDeleted);
    }

    public async Task<IEnumerable<UserResponseDTO>> GetAllAsync()
    {
        IEnumerable<User> users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserResponseDTO>>(users);
    }

    public async Task<UserResponseDTO?> GetByEmailAsync(string email)
    {
        User? user = await _userRepository.GetByEmailAsync(email);

        if (user is null) return null;

        return _mapper.Map<UserResponseDTO>(user);
    }

    public async Task<UserResponseDTO?> GetByIdAsync(int idUser)
    {
        User? user = await _userRepository.GetByIdAsync(idUser);

        if (user is null) return null;

        return _mapper.Map<UserResponseDTO>(user);
    }

    public async Task<UserResponseDTO> UpdateEmailAsync(UserUpdateEmailDTO userRequest)
    {
        bool emailInUse = await UserAlreadyRegistered(userRequest.Email);
        DomainValidationException.When(emailInUse, "E-mail já Cadastrado");
        ValidateUserId(userRequest.Id);

        User user = await GetExistingUser(userRequest.Id);

        user.Ativar();
        user.AlterarEmail(userRequest.Email);
        User updatedUser = await _userRepository.UpdateAsync(user);
        return _mapper.Map<UserResponseDTO>(updatedUser);
    }

    public async Task<UserResponseDTO> UpdatePasswordAsync(UserUpdatePasswordDTO userRequest)
    {
        ValidateUserId(userRequest.Id);

        DomainValidationException.When(!userRequest.Password.Equals(userRequest.ConfirmPassword), "Password não confere");

        User user = await GetExistingUser(userRequest.Id);

        user.Ativar();
        user.AlterarSenha(userRequest.Password);
        User updatedUser = await _userRepository.UpdateAsync(user);
        return _mapper.Map<UserResponseDTO>(updatedUser);
    }

    public async Task<UserResponseDTO> UpdatePerfilAsync(UserUpdatePerfilDTO userRequest)
    {
        ValidateUserId(userRequest.Id);

        DomainValidationException.When(userRequest.IdPerfil <= 0, "Id Perfil Inválido");

        Perfil? perfil = await _perfilRepository.GetByIdAsync(userRequest.IdPerfil);
        DomainValidationException.When(perfil is null, "Id Perfil Inválido");

        User user = await GetExistingUser(userRequest.Id);

        user.Ativar();
        user.AlterarPerfil(userRequest.IdPerfil);
        User updatedUser = await _userRepository.UpdateAsync(user);
        return _mapper.Map<UserResponseDTO>(updatedUser);
    }

    public async Task<UserResponseDTO> UpdateRegisterDataAsync(UserUpdateRegisterDTO userRequest)
    {
        DomainValidationException.When(userRequest.Id <= 0, "Id Usuário Inválido");

        User user = await GetExistingUser(userRequest.Id);

        user.Ativar();
        user.AterarNome(userRequest.Nome, userRequest.Sobrenome);
        User updatedUser = await _userRepository.UpdateAsync(user);
        return _mapper.Map<UserResponseDTO>(updatedUser);
    }

    public async Task<bool> UserAlreadyRegistered(string email)
    {
        User? user = await _userRepository.GetByEmailAsync(email);

        return user != null;
    }

    private void ValidateUserId(int id)
    {
        DomainValidationException.When(id <= 0, "Id Usuário Inválido");
    }

    private async Task<User> GetExistingUser(int idUser)
    {
        User? user = await _userRepository.GetByIdAsync(idUser);

        DomainValidationException.When(user is null, "Usuário não Encontrado");

        return user!;
    }
}