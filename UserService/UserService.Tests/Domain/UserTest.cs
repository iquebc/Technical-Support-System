using UserService.Web.API.Domain.Entities;
using UserService.Web.API.Domain.Validation;

namespace UserService.Tests.Domain;

public class UserTest
{
    public static User GetMock(int id = 1, string nome = "Jhon", string sobrenome = "Doe", string email = "jhon.doe@test.com", string senha = "Abc12345!", int idPerfil = 1, bool ativo = true)
    {
        return new User(id, nome, sobrenome, email, senha, idPerfil, ativo);
    }

    [Fact]
    public void DeveCriarUsuarioComSucesso()
    {
        User user = GetMock();

        Assert.Equal(1, user.Id);
        Assert.Equal("Jhon", user.Nome);
        Assert.Equal("Doe", user.Sobrenome);
        Assert.Equal("jhon.doe@test.com", user.Email);
        Assert.Equal("Abc12345!", user.Password);
        Assert.Equal(1, user.IdPerfil);
        Assert.True(user.Ativo);
    }

    [Fact]
    public void DeveGerarExceptionQuandoNomeInvalido()
    {
        Action action = () => GetMock(nome: "");

        DomainValidationException exception = Assert.Throws<DomainValidationException>(action);

        Assert.Equal("Nome precisa ser informado", exception.Message);
    }

    [Fact]
    public void DeveGerarExceptionQuandoSobrenomeInvalido()
    {
        Action action = () => GetMock(sobrenome: "");

        DomainValidationException exception = Assert.Throws<DomainValidationException>(action);

        Assert.Equal("Sobrenome precisa ser informado", exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData("teste")]
    [InlineData("teste@")]
    [InlineData("teste@.")]
    [InlineData("teste@teste")]
    [InlineData("teste@teste.")]
    public void DeveGerarExceptionQuandoEmailInvalido(string email)
    {
        Action action = () => GetMock(email: email);

        DomainValidationException exception = Assert.Throws<DomainValidationException>(action);

        Assert.Equal("E-mail Inválido", exception.Message);
    }

    [Fact]
    public void DeveGerarExceptionQuandoSenhaInvalido()
    {
        Action action = () => GetMock(senha: "");

        DomainValidationException exception = Assert.Throws<DomainValidationException>(action);

        Assert.Equal("Senha Inválida", exception.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void DeveGerarExceptionQuandoIdPerfilInvalido(int idPerfil)
    {
        Action action = () => GetMock(idPerfil: idPerfil);

        DomainValidationException exception = Assert.Throws<DomainValidationException>(action);

        Assert.Equal("Perfil Inválido", exception.Message);
    }

    [Fact]
    public void DeveAlterarNomeSobrenome()
    {
        User user = GetMock();
        user.AterarNome("test", "SobrenomeTest");
        Assert.Equal("test", user.Nome);
        Assert.Equal("SobrenomeTest", user.Sobrenome);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData(" ", " ")]
    [InlineData("", " ")]
    [InlineData(" ", "")]
    [InlineData("teste", "")]
    [InlineData("", "teste")]
    public void DeveGerarExceptionAlterarNomeSobrenomeInvalido(string nome, string sobrenome)
    {
        User user = GetMock();

        Action action = () => user.AterarNome(nome, sobrenome);

        DomainValidationException exception = Assert.Throws<DomainValidationException>(action);

        Assert.Equal("Nome/Sobrenome Inválido", exception.Message);
    }

    [Fact]
    public void DeveAlterarEmail()
    {
        User user = GetMock();
        user.AlterarEmail("teste@teste.com");
        Assert.Equal("teste@teste.com", user.Email);
    }

    [Theory]
    [InlineData("")]
    [InlineData("teste")]
    [InlineData("teste@")]
    [InlineData("teste@.")]
    [InlineData("teste@teste")]
    [InlineData("teste@teste.")]
    public void DeveGerarExceptionAlterarEmailInvalido(string email)
    {
        User user = GetMock();

        Action action = () => user.AlterarEmail(email);

        DomainValidationException exception = Assert.Throws<DomainValidationException>(action);

        Assert.Equal("E-mail Inválido", exception.Message);
    }

    [Fact]
    public void DeveAlterarSenha()
    {
        User user = GetMock();
        user.AlterarSenha("NovaSenha");
        Assert.Equal("NovaSenha", user.Password);
    }


    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void DeveGerarExceptionAlterarSenhaInvalido(string senha)
    {
        User user = GetMock();

        Action action = () => user.AlterarSenha(senha);

        DomainValidationException exception = Assert.Throws<DomainValidationException>(action);

        Assert.Equal("Senha Inválida", exception.Message);
    }

    [Fact]
    public void DeveAlterarPerfil()
    {
        User user = GetMock();
        user.AlterarPerfil(2);
        Assert.Equal(2, user.IdPerfil);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void DeveGerarExceptionAlterarPerfilInvalido(int idPerfil)
    {
        User user = GetMock();

        Action action = () => user.AlterarPerfil(idPerfil);

        DomainValidationException exception = Assert.Throws<DomainValidationException>(action);

        Assert.Equal("Perfil Inválido", exception.Message);
    }
}
