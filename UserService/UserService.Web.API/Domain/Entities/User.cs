using UserService.Web.API.Domain.Validation;

namespace UserService.Web.API.Domain.Entities;

public class User : Entity
{
    public string Nome { get; private set; }
    public string Sobrenome { get; private set; }
    public string Email { get; private set; }
    public string Senha { get; private set; }
    public int IdPerfil { get; private set; }
    public virtual Perfil? Perfil { get; set; }

    public User(int id, string nome, string sobrenome, string email, string senha, int idPerfil, bool ativo) : base(id, ativo)
    {
        DomainValidationException.When(string.IsNullOrWhiteSpace(nome), "Nome precisa ser informado");

        DomainValidationException.When(string.IsNullOrWhiteSpace(sobrenome), "Sobrenome precisa ser informado");

        DomainValidationException.When(string.IsNullOrWhiteSpace(email), "Email precisa ser informado");

        DomainValidationException.When(string.IsNullOrWhiteSpace(senha), "Senha precisa ser informado");

        DomainValidationException.When(idPerfil < 0, "Perfil inválido");

        Nome = nome;
        Sobrenome = sobrenome;
        Senha = senha;
        Email = email;
        IdPerfil = idPerfil;
    }

    public void AterarNome(string nome, string sobrenome)
    {
        DomainValidationException.When(string.IsNullOrWhiteSpace(nome), "Nome precisa ser informado");
        DomainValidationException.When(string.IsNullOrWhiteSpace(sobrenome), "Sobrenome precisa ser informado");

        Nome = nome;
        Sobrenome = sobrenome;
    }

    public void AlterarEmail(string email)
    {
        DomainValidationException.When(string.IsNullOrWhiteSpace(email), "Email precisa ser informado");

        Email = email;
    }

    public void AlterarSenha(string senha)
    {
        DomainValidationException.When(string.IsNullOrWhiteSpace(senha), "Senha precisa ser informado");

        Senha = senha;
    }

    public void AlterarPerfil(int idPerfil)
    {
        DomainValidationException.When(idPerfil < 0, "Perfil inválido");

        IdPerfil = idPerfil;
    }
}