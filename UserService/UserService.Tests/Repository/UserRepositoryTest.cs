using UserService.Tests.Domain;
using UserService.Web.API.Domain.Entities;
using UserService.Web.API.Domain.Interfaces;
using UserService.Web.API.Infrastructure.Context;
using UserService.Web.API.Infrastructure.IoC;
using UserService.Web.API.Infrastructure.Repositories;

namespace UserService.Tests.Repository;

public class UserRepositoryTest
{
    private readonly IUserRepository _repository;

    private readonly ApplicationDbContext _context;

    public UserRepositoryTest()
    {
        DBInMemory dBInMemory = new DBInMemory();
        _context = dBInMemory.GetContext();
        _repository = new UserRepository(_context);

        CleanDatabase(_context);
        SeedDefaultData(_context);
    }

    public static void CleanDatabase(ApplicationDbContext _context)
    {
        _context.Users.RemoveRange(_context.Users);
        _context.SaveChanges();
    }

    public static void SeedDefaultData(ApplicationDbContext _context)
    {
        var defaultUser = UserTest.GetMock();
        if (!_context.Users.Any())
        {
            _context.Users.Add(defaultUser);
            _context.SaveChanges();
        }
    }

    [Fact]
    public async Task GetAllAsync()
    {
        IEnumerable<User> users = await _repository.GetAllAsync();
        Assert.Single(users);
    }

    [Fact]
    public async Task GetbyIdAsync()
    {
        User? user = await _repository.GetByIdAsync(1);
        Assert.NotNull(user);
    }

    [Fact]
    public async Task GetbyEmailAsync()
    {
        User? user = await _repository.GetByEmailAsync("jhon.doe@test.com");
        Assert.NotNull(user);
    }

    [Fact]
    public async Task CreateAsync()
    {
        User? user = await _repository.CreateAsync(UserTest.GetMock(id: 0, email: "test@test.com"));
        Assert.NotNull(user);
        Assert.NotEqual(0, user.Id);
        Assert.Equal("test@test.com", user.Email);
        Assert.True(user.Ativo);
    }

    [Fact]
    public async Task UpdateAsync()
    {
        User? user = await _repository.GetByIdAsync(1);
        Assert.NotNull(user);
        user.AlterarEmail("emailNovo@email.com");

        await _repository.UpdateAsync(user);
        User? userAlterado = await _repository.GetByIdAsync(1);

        Assert.NotNull(userAlterado);
        Assert.Equal(1, userAlterado.Id);
        Assert.Equal("emailNovo@email.com", userAlterado.Email);
        Assert.True(userAlterado.Ativo);
    }

    [Fact]
    public async Task DeleteAsync()
    {
        User? user = await _repository.GetByIdAsync(1);
        Assert.NotNull(user);
        user.Inativar();

        await _repository.DeleteAsync(user);
        User? userInativo = await _repository.GetByIdAsync(1);
        Assert.NotNull(userInativo);
        Assert.Equal(1, userInativo.Id);
        Assert.False(userInativo.Ativo);
    }


}
