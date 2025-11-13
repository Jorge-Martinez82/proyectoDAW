using AdoptameDAW.Models.DTOs;
using AdoptameDAW.Models;
using AutoMapper;
using AdoptameDAW.Repositories;

namespace AdoptameDAW.Services;

public class ProtectorasService
{
    private readonly IProtectorasRepository _repository;
    private readonly IMapper _mapper;

    public ProtectorasService(IProtectorasRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProtectoraDto?> GetByIdAsync(Guid id)
    {
        var protectora = await _repository.GetByIdAsync(id);
        return protectora == null ? null : _mapper.Map<ProtectoraDto>(protectora);
    }

    public async Task<object> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? provincia = null)
    {
        pageSize = pageSize > 12 ? 12 : pageSize;
        pageNumber = pageNumber < 1 ? 1 : pageNumber;

        var (protectoras, total) = await _repository.GetAllAsync(
            pageNumber,
            pageSize,
            provincia);

        var protectorasDto = _mapper.Map<IEnumerable<ProtectoraDto>>(protectoras);

        return new
        {
            data = protectorasDto,
            pageNumber,
            pageSize,
            totalCount = total,
            totalPages = (int)Math.Ceiling(total / (double)pageSize)
        };
    }

    
}
