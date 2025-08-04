using UserService.Tests.Domain;
using UserService.Web.API.Domain.Entities;
using UserService.Web.API.Domain.Interfaces;
using UserService.Web.API.Infrastructure.Context;
using UserService.Web.API.Infrastructure.IoC;
using UserService.Web.API.Infrastructure.Repositories;

namespace UserService.Tests.Repository;

public class PerfilRepositoryTest
{
    private readonly IPerfilRepository _repository;

    private readonly ApplicationDbContext _context;

    public PerfilRepositoryTest()
    {
        DBInMemory dBInMemory = new DBInMemory();
        _context = dBInMemory.GetContext();
        _repository = new PerfilRepository(_context);

        CleanDatabase(_context);
        SeedDefaultData(_context);
    }

    public static void CleanDatabase(ApplicationDbContext _context)
    {
        _context.Perfil.RemoveRange(_context.Perfil);
        _context.SaveChanges();
    }

    public static void SeedDefaultData(ApplicationDbContext _context)
    {
        if (!_context.Perfil.Any())
        {
            _context.Perfil.Add(PerfilTest.GetMock());
            _context.SaveChanges();
        }
    }

    [Fact]
    public async Task GetAllAsync()
    {
        IEnumerable<Perfil> perfis = await _repository.GetAllAsync();
        Assert.Single(perfis);
    }

    [Fact]
    public async Task GetbyIdAsync()
    {
        Perfil? perfil = await _repository.GetByIdAsync(1);
        Assert.NotNull(perfil);
    }
}
