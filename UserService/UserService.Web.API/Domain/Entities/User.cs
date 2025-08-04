using UserService.Web.API.Domain.Validation;

namespace UserService.Web.API.Domain.Entities;

public class User : Entity
{
    public string Nome { get; private set; }
    public string Sobrenome { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public int IdPerfil { get; private set; }
    public virtual Perfil? Perfil { get; set; }

    public User(int id, string nome, string sobrenome, string email, string password, int idPerfil, bool ativo) : base(id, ativo)
    {
        DomainValidationException.When(string.IsNullOrWhiteSpace(nome), "Nome precisa ser informado");

        DomainValidationException.When(string.IsNullOrWhiteSpace(sobrenome), "Sobrenome precisa ser informado");

        DomainValidationException.When(string.IsNullOrWhiteSpace(email), "E-mail Inválido");

        DomainValidationException.When(!EmailValidation.IsValidEmail(email), "E-mail Inválido");

        DomainValidationException.When(string.IsNullOrWhiteSpace(password), "Senha Inválida");

        DomainValidationException.When(idPerfil <= 0, "Perfil Inválido");

        Nome = nome;
        Sobrenome = sobrenome;
        Password = password;
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

    public void AlterarSenha(string password)
    {
        DomainValidationException.When(string.IsNullOrWhiteSpace(password), "Senha Inválida");

        Password = password;
    }

    public void AlterarPerfil(int idPerfil)
    {
        DomainValidationException.When(idPerfil <= 0, "Perfil Inválido");

        IdPerfil = idPerfil;
    }
}