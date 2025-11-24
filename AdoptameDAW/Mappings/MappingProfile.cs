using AutoMapper;
using AdoptameDAW.Models;
using AdoptameDAW.Models.DTOs;

namespace AdoptameDAW.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Animal, AnimalDto>().ReverseMap();
        CreateMap<Protectora, ProtectoraDto>().ReverseMap();
        CreateMap<Usuario, UsuarioDto>().ReverseMap();
        CreateMap<Adoptante, AdoptanteDto>().ReverseMap();
        CreateMap<Solicitud, SolicitudDto>()
            .ForMember(d => d.AnimalUuid, o => o.MapFrom(s => s.Animal.Uuid))
            .ForMember(d => d.UsuarioAdoptanteUuid, o => o.MapFrom(s => s.UsuarioAdoptante.Uuid))
            .ForMember(d => d.UsuarioProtectoraUuid, o => o.MapFrom(s => s.UsuarioProtectora.Uuid));
    }
}
