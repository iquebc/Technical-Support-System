using UserService.Web.API.Domain.Entities;

namespace UserService.Tests.Domain;

public class PerfilTest
{
    public static Perfil GetMock(int id = 1, string descricao = "Sem Perfil", bool ativo = true)
    {
        return new Perfil(id, descricao, ativo);
    }
}