using UserService.Web.API.Domain.Entities;
using UserService.Web.API.Domain.Interfaces;

namespace UserService.Tests.Repository;

public class PerfilRepositoryTest
{
    private readonly IPerfilRepository _repository;

    private readonly ApplicationTestFixture _fixture;

    public PerfilRepositoryTest()
    {
        _fixture = new ApplicationTestFixture();
        _repository = _fixture.PerfilRepository;
    }

    [Fact]
    public async Task GetAllAsync()
    {
        IEnumerable<Perfil> perfis = await _repository.GetAllAsync();
        Assert.Equal(2, perfis.Count());
    }

    [Fact]
    public async Task GetbyIdAsync()
    {
        Perfil? perfil = await _repository.GetByIdAsync(1);
        Assert.NotNull(perfil);
    }
}
