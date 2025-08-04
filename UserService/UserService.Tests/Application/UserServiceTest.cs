using AutoMapper;
using UserService.Tests.Repository;
using UserService.Web.API.Application.DTOs;
using UserService.Web.API.Application.Interfaces;
using UserService.Web.API.Application.Mapping;
using UserService.Web.API.Domain.Entities;
using UserService.Web.API.Domain.Interfaces;
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
}