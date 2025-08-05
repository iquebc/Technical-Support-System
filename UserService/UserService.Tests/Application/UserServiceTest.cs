using AutoMapper;
using UserService.Tests.Repository;
using UserService.Web.API.Application.DTOs;
using UserService.Web.API.Application.Interfaces;
using UserService.Web.API.Application.Mapping;
using UserService.Web.API.Domain.Interfaces;
using UserService.Web.API.Domain.Validation;
using UserService.Web.API.Infrastructure.Context;
using UserService.Web.API.Infrastructure.IoC;
using UserService.Web.API.Infrastructure.Repositories;

namespace UserService.Tests.Application;

public class UserServiceTest
{
    private readonly IUserRepository _userRepository;
    private readonly ApplicationDbContext _context;
    private readonly IPerfilRepository _perfilRepository;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public UserServiceTest()
    {
        _context = new DBInMemory().GetContext();
        UserRepositoryTest.CleanDatabase(_context);
        UserRepositoryTest.SeedDefaultData(_context);

        _userRepository = new UserRepository(_context);
        _perfilRepository = new PerfilRepository(_context);
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new DomainToDTOMappingProfile())).CreateMapper();
        _userService = new UserService.Web.API.Application.Services.UserService(_userRepository, _mapper, _perfilRepository);
    }

    public UserRequestDTO GetMock(int id = 0, string name = "Test", string sobrenome = "Test", string email = "test@test.com", string password = "Test123", string confirmPassword = "Test123")
    {
        return new UserRequestDTO
        {
            Nome = name,
            Sobrenome = sobrenome,
            Email = email,
            Password = password,
            ConfirmPassword = confirmPassword
        };
    }

    [Fact]
    public async Task Should_Create_User_Successfully()
    {

        UserRequestDTO userRequest = GetMock();

        UserResponseDTO createdUser = await _userService.CreateAsync(userRequest);

        Assert.NotNull(createdUser);
        Assert.NotNull(createdUser.Perfil);
        Assert.NotEqual(0, createdUser.Id);
        Assert.Equal(userRequest.Nome, createdUser.Nome);
        Assert.Equal(userRequest.Sobrenome, createdUser.Sobrenome);
        Assert.Equal(userRequest.Email, createdUser.Email);
        Assert.True(createdUser.Ativo);
        Assert.Equal(1, createdUser.Perfil.Id);
    }

    [Theory]
    [InlineData("JHON.DOE@TEST.COM")]
    [InlineData("jhon.doe@test.com")]
    public async Task CreateUserAsync_ShouldThrowException_WhenUserAlreadyExists(string email)
    {
        UserRequestDTO userRequest = GetMock(email: email);

        Func<Task> action = () => _userService.CreateAsync(userRequest);

        DomainValidationException exception = await Assert.ThrowsAsync<DomainValidationException>(action);

        Assert.Equal("E-mail já Cadastrado", exception.Message);
    }

    [Fact]
    public async Task Should_Delete_User_Successfully()
    {
        UserResponseDTO user = await _userService.DeleteAsync(1);

        Assert.NotNull(user);
        Assert.Equal(1, user.Id);
        Assert.False(user.Ativo);
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldThrowException_WhenUserDoesNotExist()
    {
        Func<Task> action = () => _userService.DeleteAsync(999);

        DomainValidationException exception = await Assert.ThrowsAsync<DomainValidationException>(action);

        Assert.Equal("Usuário não encontrado", exception.Message);
    }

    [Fact]
    public async Task Should_Return_All_Users_Successfully()
    {
        IEnumerable<UserResponseDTO> users = await _userService.GetAllAsync();
        Assert.NotNull(users);
        Assert.NotEmpty(users);
        Assert.Single(users);
    }

    [Theory]
    [InlineData("JHON.DOE@TEST.COM")]
    [InlineData("jhon.doe@test.com")]
    public async Task Should_Return_User_By_Email_Successfully(string email)
    {
        UserResponseDTO? user = await _userService.GetByEmailAsync(email);

        Assert.NotNull(user);
        Assert.NotNull(user.Perfil);
        Assert.Equal(1, user.Id);
        Assert.True(user.Ativo);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        UserResponseDTO? user = await _userService.GetByEmailAsync("test@tst.com");

        Assert.Null(user);
    }

    [Fact]
    public async Task Should_Return_User_By_Id_Successfully()
    {
        UserResponseDTO? user = await _userService.GetByIdAsync(1);
        Assert.NotNull(user);
        Assert.NotNull(user.Perfil);
        Assert.Equal(1, user.Id);
        Assert.True(user.Ativo);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        UserResponseDTO? user = await _userService.GetByIdAsync(999);
        Assert.Null(user);
    }

    [Fact]
    public async Task Should_Update_User_Email_Successfully()
    {
        UserUpdateEmailDTO userRequest = new UserUpdateEmailDTO
        {
            Id = 1,
            Email = "teste@teste.com"
        };
        UserResponseDTO updatedUser = await _userService.UpdateEmailAsync(userRequest);

        Assert.NotNull(updatedUser);
        Assert.Equal(userRequest.Id, updatedUser.Id);
        Assert.Equal(userRequest.Email, updatedUser.Email);
        Assert.True(updatedUser.Ativo);
    }

    [Theory]
    [InlineData("JHON.DOE@TEST.COM")]
    [InlineData("jhon.doe@test.com")]
    public async Task UpdateEmailAsync_ShouldThrowException_WhenEmailAlreadyInUse(string email)
    {
        UserUpdateEmailDTO userRequest = new UserUpdateEmailDTO
        {
            Id = 1,
            Email = email
        };
        Func<Task> action = () => _userService.UpdateEmailAsync(userRequest);

        DomainValidationException exception = await Assert.ThrowsAsync<DomainValidationException>(action);
        Assert.Equal("E-mail já Cadastrado", exception.Message);
    }

    [Fact]
    public async Task UpdateEmailAsync_ShouldThrowException_WhenUserDoesNotExist()
    {
        UserUpdateEmailDTO userRequest = new UserUpdateEmailDTO
        {
            Id = 999,
            Email = "abc@abc.com"
        };

        Func<Task> action = () => _userService.UpdateEmailAsync(userRequest);
        DomainValidationException exception = await Assert.ThrowsAsync<DomainValidationException>(action);
        Assert.Equal("Usuário não encontrado", exception.Message);
    }

    [Fact]
    public async Task Should_Update_User_Password_Successfully()
    {
        UserUpdatePasswordDTO userRequest = new UserUpdatePasswordDTO
        {
            Id = 1,
            Password = "NewPassword123",
            ConfirmPassword = "NewPassword123"
        };
        UserResponseDTO updatedUser = await _userService.UpdatePasswordAsync(userRequest);
        Assert.NotNull(updatedUser);
        Assert.Equal(userRequest.Id, updatedUser.Id);
        Assert.True(updatedUser.Ativo);
    }

    [Fact]
    public async Task UpdatePasswordAsync_ShouldThrowException_WhenPasswordsDoNotMatch()
    {
        UserUpdatePasswordDTO userRequest = new UserUpdatePasswordDTO
        {
            Id = 1,
            Password = "NewPassword123",
            ConfirmPassword = "DifferentPassword123"
        };
        Func<Task> action = () => _userService.UpdatePasswordAsync(userRequest);
        DomainValidationException exception = await Assert.ThrowsAsync<DomainValidationException>(action);
        Assert.Equal("Password não confere", exception.Message);
    }

    [Fact]
    public async Task UpdatePasswordAsync_ShouldThrowException_WhenUserDoesNotExist()
    {
        UserUpdatePasswordDTO userRequest = new UserUpdatePasswordDTO
        {
            Id = 999,
            Password = "NewPassword123",
            ConfirmPassword = "NewPassword123"
        };
        Func<Task> action = () => _userService.UpdatePasswordAsync(userRequest);
        DomainValidationException exception = await Assert.ThrowsAsync<DomainValidationException>(action);
        Assert.Equal("Usuário não encontrado", exception.Message);
    }

    [Fact]
    public async Task Should_Update_User_Perfil_Successfully()
    {
        UserUpdatePerfilDTO userRequest = new UserUpdatePerfilDTO
        {
            Id = 1,
            IdPerfil = 2
        };
        UserResponseDTO updatedUser = await _userService.UpdatePerfilAsync(userRequest);
        Assert.NotNull(updatedUser);
        Assert.Equal(userRequest.Id, updatedUser.Id);
        Assert.Equal(userRequest.IdPerfil, updatedUser.Perfil.Id);
        Assert.True(updatedUser.Ativo);
    }
    [Fact]
    public async Task UpdatePerfilAsync_ShouldThrowException_WhenPerfilDoesNotExist()
    {
        UserUpdatePerfilDTO userRequest = new UserUpdatePerfilDTO
        {
            Id = 1,
            IdPerfil = 999
        };
        Func<Task> action = () => _userService.UpdatePerfilAsync(userRequest);
        DomainValidationException exception = await Assert.ThrowsAsync<DomainValidationException>(action);
        Assert.Equal("Id Perfil Inválido", exception.Message);
    }
    [Fact]
    public async Task UpdatePerfilAsync_ShouldThrowException_WhenUserDoesNotExist()
    {
        UserUpdatePerfilDTO userRequest = new UserUpdatePerfilDTO
        {
            Id = 999,
            IdPerfil = 2
        };
        Func<Task> action = () => _userService.UpdatePerfilAsync(userRequest);
        DomainValidationException exception = await Assert.ThrowsAsync<DomainValidationException>(action);
        Assert.Equal("Usuário não encontrado", exception.Message);
    }

    [Fact]
    public async Task UpdatePerfilAsync_ShouldThrowException_WhenPerfilIdIsInvalid()
    {
        UserUpdatePerfilDTO userRequest = new UserUpdatePerfilDTO
        {
            Id = 1,
            IdPerfil = 0
        };
        Func<Task> action = () => _userService.UpdatePerfilAsync(userRequest);
        DomainValidationException exception = await Assert.ThrowsAsync<DomainValidationException>(action);
        Assert.Equal("Id Perfil Inválido", exception.Message);
    }

    [Fact]
    public async Task Should_Update_User_Register_Data_Successfully()
    {
        UserUpdateRegisterDTO userRequest = new UserUpdateRegisterDTO
        {
            Id = 1,
            Nome = "UpdatedName",
            Sobrenome = "UpdatedSurname"
        };
        UserResponseDTO updatedUser = await _userService.UpdateRegisterDataAsync(userRequest);
        Assert.NotNull(updatedUser);
        Assert.Equal(userRequest.Id, updatedUser.Id);
        Assert.Equal(userRequest.Nome, updatedUser.Nome);
        Assert.Equal(userRequest.Sobrenome, updatedUser.Sobrenome);
        Assert.True(updatedUser.Ativo);
    }

    [Fact]
    public async Task UpdateRegisterDataAsync_ShouldThrowException_WhenUserDoesNotExist()
    {
        UserUpdateRegisterDTO userRequest = new UserUpdateRegisterDTO
        {
            Id = 999,
            Nome = "UpdatedName",
            Sobrenome = "UpdatedSurname"
        };
        Func<Task> action = () => _userService.UpdateRegisterDataAsync(userRequest);
        DomainValidationException exception = await Assert.ThrowsAsync<DomainValidationException>(action);
        Assert.Equal("Usuário não encontrado", exception.Message);
    }

    [Theory]
    [InlineData("JHON.DOE@TEST.COM")]
    [InlineData("jhon.doe@test.com")]
    public async Task UserAlreadyRegistered_ShouldReturnTrue_WhenEmailExists(string email)
    {
        bool exists = await _userService.UserAlreadyRegistered(email);
        Assert.True(exists);
    }

    [Fact]
    public async Task UserAlreadyRegistered_ShouldReturnFalse_WhenEmailDoesNotExist()
    {
        bool exists = await _userService.UserAlreadyRegistered("teste@teste.com");
        Assert.False(exists);
    }
}