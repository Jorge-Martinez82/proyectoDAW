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
    }
}
