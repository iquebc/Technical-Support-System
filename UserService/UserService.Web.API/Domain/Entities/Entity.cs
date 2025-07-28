using UserService.Web.API.Domain.Validation;

namespace UserService.Web.API.Domain.Entities;

public abstract class Entity
{
    private int id;
    public int Id
    {
        get { return id; }
        protected set
        {
            DomainValidationException.When(value < 0, "Invalid Id Value");
            id = value;
        }
    }

    private bool ativo;
    public bool Ativo
    {
        get { return ativo; }
        protected set { ativo = value; }
    }

    public Entity(int id, bool ativo)
    {
        Id = id;
        Ativo = ativo;
    }

    public void Inativar()
    {
        this.Ativo = false;
    }

    public void Ativar()
    {
        this.Ativo = true;
    }
}