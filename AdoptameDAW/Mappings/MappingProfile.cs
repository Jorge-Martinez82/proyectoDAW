using AutoMapper;
using AdoptameDAW.Models;
using AdoptameDAW.Models.DTOs;

namespace AdoptameDAW.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Animal, AnimalDto>()
            .ForMember(d => d.ProtectoraNombre, o => o.MapFrom(s => s.Protectora.Nombre))
            .ForMember(d => d.ProtectoraEmail, o => o.MapFrom(s => s.Protectora.Email))
            .ForMember(d => d.ProtectoraProvincia, o => o.MapFrom(s => s.Protectora.Provincia))
            .ReverseMap();
        
        CreateMap<Protectora, ProtectoraDto>().ReverseMap();
        CreateMap<Usuario, UsuarioDto>().ReverseMap();
        CreateMap<Adoptante, AdoptanteDto>().ReverseMap();
        CreateMap<Solicitud, SolicitudDto>()
            .ForMember(d => d.AnimalUuid, o => o.MapFrom(s => s.Animal.Uuid))
            .ForMember(d => d.UsuarioAdoptanteUuid, o => o.MapFrom(s => s.UsuarioAdoptante.Uuid))
            .ForMember(d => d.UsuarioProtectoraUuid, o => o.MapFrom(s => s.UsuarioProtectora.Uuid))
            .ForMember(d => d.AdoptanteNombre, o => o.MapFrom(s => s.UsuarioAdoptante.Email)) // fallback si no hay perfil
            .ForMember(d => d.AdoptanteEmail, o => o.MapFrom(s => s.UsuarioAdoptante.Email));
    }
}
