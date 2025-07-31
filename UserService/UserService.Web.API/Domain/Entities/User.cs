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

        DomainValidationException.When(string.IsNullOrWhiteSpace(email), "E-mail Inválido");

        DomainValidationException.When(!EmailValidation.IsValidEmail(email), "E-mail Inválido");

        DomainValidationException.When(string.IsNullOrWhiteSpace(senha), "Senha Inválida");

        DomainValidationException.When(idPerfil <= 0, "Perfil Inválido");

        Nome = nome;
        Sobrenome = sobrenome;
        Senha = senha;
        Email = email;
        IdPerfil = idPerfil;
    }

    public void AterarNome(string nome, string sobrenome)
    {
        DomainValidationException.When(string.IsNullOrWhiteSpace(nome), "Nome/Sobrenome Inválido");

        DomainValidationException.When(string.IsNullOrWhiteSpace(sobrenome), "Nome/Sobrenome Inválido");

        Nome = nome;
        Sobrenome = sobrenome;
    }

    public void AlterarEmail(string email)
    {
        DomainValidationException.When(string.IsNullOrWhiteSpace(email), "E-mail Inválido");

        DomainValidationException.When(!EmailValidation.IsValidEmail(email), "E-mail Inválido");

        Email = email;
    }

    public void AlterarSenha(string senha)
    {
        DomainValidationException.When(string.IsNullOrWhiteSpace(senha), "Senha Inválida");

        Senha = senha;
    }

    public void AlterarPerfil(int idPerfil)
    {
        DomainValidationException.When(idPerfil <= 0, "Perfil Inválido");

        IdPerfil = idPerfil;
    }
}