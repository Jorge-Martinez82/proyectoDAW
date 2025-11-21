using AdoptameDAW.Models;
using AdoptameDAW.Models.DTOs;
using AdoptameDAW.Repositories;
using AutoMapper;

namespace AdoptameDAW.Services
{
    public class AdoptantesService
    {
        private readonly IAdoptantesRepository _repository;
        private readonly IMapper _mapper;

        public AdoptantesService(IAdoptantesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AdoptanteDto?> GetByUsuarioUuidAsync(Guid usuarioUuid)
        {
            var adoptante = await _repository.GetByUsuarioUuidAsync(usuarioUuid);
            return adoptante == null ? null : _mapper.Map<AdoptanteDto>(adoptante);
        }

        public async Task<AdoptanteDto?> GetByAdoptanteUuidAsync(Guid adoptanteUuid)
        {
            var adoptante = await _repository.GetByUuidAsync(adoptanteUuid);
            return adoptante == null ? null : _mapper.Map<AdoptanteDto>(adoptante);
        }

        public async Task<object> GetAllAsync(int pageNumber, int pageSize)
        {
            pageSize = pageSize > 12 ? 12 : pageSize;
            pageNumber = pageNumber < 1 ? 1 : pageNumber;

            var (adoptantes, total) = await _repository.GetAllAsync(pageNumber, pageSize);
            var adoptantesDto = _mapper.Map<IEnumerable<AdoptanteDto>>(adoptantes);

            return new
            {
                data = adoptantesDto,
                pageNumber,
                pageSize,
                totalCount = total,
                totalPages = (int)Math.Ceiling(total / (double)pageSize)
            };
        }

        public async Task<bool> UpdateAsync(Guid adoptanteUuid, AdoptanteDto dto)
        {
            var entidad = await _repository.GetByUuidAsync(adoptanteUuid);
            if (entidad == null) return false;

            _mapper.Map(dto, entidad);
            return await _repository.UpdateAsync(entidad);
        }

        public async Task<AdoptanteDto> CreateAsync(AdoptanteDto dto)
        {
            var entidad = _mapper.Map<Adoptante>(dto);
            var creado = await _repository.CreateAsync(entidad);
            return _mapper.Map<AdoptanteDto>(creado);
        }
    }
}
