using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Web.API.Domain.Entities;

namespace UserService.Web.API.Infrastructure.EntitiesConfiguration;

public class PerfilConfiguration : IEntityTypeConfiguration<Perfil>
{
    public void Configure(EntityTypeBuilder<Perfil> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasData(new Perfil(1, "Sem Perfil", true));
        builder.HasData(new Perfil(2, "Administrador", true));
        builder.HasData(new Perfil(3, "Usuário", true));
    }
}