using AutoMapper;
using UserService.Tests.Domain;
using UserService.Web.API.Application.Interfaces;
using UserService.Web.API.Application.Mapping;
using UserService.Web.API.Domain.Interfaces;
using UserService.Web.API.Infrastructure.Context;
using UserService.Web.API.Infrastructure.IoC;
using UserService.Web.API.Infrastructure.Repositories;

namespace UserService.Tests;

public class ApplicationTestFixture
{
    public ApplicationDbContext DbContext { get; private set; }

    public IMapper Mapper { get; private set; }

    public IUserRepository UserRepository { get; private set; }

    public IPerfilRepository PerfilRepository { get; private set; }

    public IUserService UserService { get; private set; }

    public ApplicationTestFixture()
    {
        DbContext = new DBInMemory().GetContext();
        ResetDatabase();
        SeedDatabase();

        Mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new DomainToDTOMappingProfile());
        })
        .CreateMapper();

        InitializeRepositories();
        InitializeServices();
    }

    public void SeedDatabase()
    {
        if (!DbContext.Perfil.Any())
        {
            DbContext.Perfil.Add(PerfilTest.GetMock());
            DbContext.Perfil.Add(PerfilTest.GetMock(id:2, descricao:"Administrador"));
        }

        if (!DbContext.Users.Any())
            DbContext.Users.Add(UserTest.GetMock());

        DbContext.SaveChanges();
    }

    public void ResetDatabase()
    {
        DbContext.Users.RemoveRange(DbContext.Users);
        DbContext.Perfil.RemoveRange(DbContext.Perfil);
        DbContext.SaveChanges();
    }

    public void InitializeRepositories()
    {
        UserRepository = new UserRepository(DbContext);
        PerfilRepository = new PerfilRepository(DbContext);
    }

    public void InitializeServices()
    {
        UserService = new Web.API.Application.Services.UserService(UserRepository, Mapper, PerfilRepository);
    }
}