using UserService.Web.API.Domain.Validation;

namespace UserService.Web.API.Domain.Entities;

public class Perfil : Entity
{
    public string Descricao { get; private set; }

    public Perfil(int id, string descricao, bool ativo) : base(id, ativo)
    {
        DomainValidationException.When(string.IsNullOrWhiteSpace(descricao), "Descrição do perfil precisa ser preenchida");

        Descricao = descricao;
    }

    public virtual ICollection<User>? Users { get; set; }
}