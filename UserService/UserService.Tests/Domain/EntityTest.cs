using UserService.Web.API.Domain.Entities;
using UserService.Web.API.Domain.Validation;

namespace UserService.Test.Domain
{

    public class EntityTestClass(int id, bool ativo) : Entity(id, ativo)
    {

    }

    public class EntityTest
    {
        public static EntityTestClass NewEntityClass(int id = 1, bool ativo = true)
        {
            return new EntityTestClass(id, ativo);
        }

        [Fact]
        public void DeveGerarUmaEntidadeValida()
        {
            EntityTestClass newEntity = NewEntityClass();
            Assert.Equal(1, newEntity.Id);
            Assert.True(newEntity.Ativo);
        }

        [Fact]
        public void DeveGerarUmDomainValidationExceptionQuandoIdMenorQueZero()
        {
            Action action = () => NewEntityClass(id: -1);
            DomainValidationException exception = Assert.Throws<DomainValidationException>(action);
            Assert.Equal("Invalid Id Value", exception.Message);
        }

        [Fact]
        public void DeveInativarUmaEntidade()
        {
            EntityTestClass newEntity = NewEntityClass();
            newEntity.Inativar();
            Assert.False(newEntity.Ativo);
        }

        [Fact]
        public void DeveAtivarUmaEntidadeInativada()
        {
            EntityTestClass newEntity = NewEntityClass(ativo: false);
            newEntity.Ativar();
            Assert.True(newEntity.Ativo);
        }
    }
}