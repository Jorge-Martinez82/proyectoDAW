using AdoptameDAW.Models.DTOs;
using AdoptameDAW.Models;
using AdoptameDAW.Repositories;
using AutoMapper;

namespace AdoptameDAW.Services;

public class AnimalesService
{
    private readonly IAnimalesRepository _repository;
    private readonly IProtectorasRepository _protectorasRepository;
    private readonly IMapper _mapper;

    public AnimalesService(IAnimalesRepository repository, IMapper mapper, IProtectorasRepository protectorasRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _protectorasRepository = protectorasRepository;
    }

    // metodo que obtiene un animal por su uuid desde el repositorio
    public async Task<AnimalDto?> GetByIdAsync(Guid id)
    {
        var animal = await _repository.GetByUuidAsync(id);
        return animal == null ? null : _mapper.Map<AnimalDto>(animal);
    }

    // metodo que obtiene animales con filtros y paginacion desde el repositorio
    public async Task<object> GetAllAsync(
        int pageNumber,
        int pageSize,
        Guid? protectoraUuid = null,
        string? tipo = null,
        string? provincia = null)
    {

        pageSize = pageSize > 12 ? 12 : pageSize;
        pageNumber = pageNumber < 1 ? 1 : pageNumber;

        var (animales, total) = await _repository.GetAllAsync(
            pageNumber,
            pageSize,
            protectoraUuid,
            tipo,
            provincia);

        var animalesDto = _mapper.Map<IEnumerable<AnimalDto>>(animales);

        return new
        {
            data = animalesDto,
            pageNumber,
            pageSize,
            totalCount = total,
            totalPages = (int)Math.Ceiling(total / (double)pageSize)
        };
    }

    // metodo que elimina un animal por su uuid
    public async Task<bool> DeleteAsync(Guid uuid)
    {
        return await _repository.DeleteAsync(uuid);
    }

    // metodo que crea un nuevo animal en el repositorio
    public async Task<AnimalDto?> CreateAsync(AnimalDto dto)
    {
        var protectora = await _protectorasRepository.GetByUuidAsync(dto.Uuid == Guid.Empty ? Guid.Empty : dto.Uuid);
        if (protectora == null && dto.ProtectoraId == 0) return null;

        var animal = _mapper.Map<Animal>(dto);

        animal.Uuid = Guid.NewGuid();
        animal.ProtectoraId = dto.ProtectoraId != 0 ? dto.ProtectoraId : (protectora?.Id ?? 0);

        animal.Nombre = string.IsNullOrWhiteSpace(animal.Nombre) ? string.Empty : animal.Nombre;
        animal.Tipo = string.IsNullOrWhiteSpace(animal.Tipo) ? string.Empty : animal.Tipo;


        var creado = await _repository.CreateAsync(animal);
        return _mapper.Map<AnimalDto>(creado);
    }
}
