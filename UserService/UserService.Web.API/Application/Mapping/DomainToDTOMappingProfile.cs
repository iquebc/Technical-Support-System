using AutoMapper;
using UserService.Web.API.Application.DTOs;
using UserService.Web.API.Domain.Entities;

namespace UserService.Web.API.Application.Mapping;

public class DomainToDTOMappingProfile : Profile
{
    public DomainToDTOMappingProfile()
    {
        CreateMap<PerfilDTO, Perfil>().ReverseMap();
        CreateMap<UserRequestDTO, User>()
            .ConstructUsing(dto => new User(0, dto.Nome, dto.Sobrenome, dto.Email, dto.Password, 1, false))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IdPerfil, opt => opt.Ignore())
            .ForMember(dest => dest.Ativo, opt => opt.Ignore())
            .ReverseMap();
        CreateMap<UserResponseDTO, User>().ReverseMap();
    }
}
