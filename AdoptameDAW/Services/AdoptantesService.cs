using AdoptameDAW.Data; 
using AdoptameDAW.Models;
using AdoptameDAW.Models.DTOs;
using AdoptameDAW.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AdoptameDAW.Services
{
    public class AdoptantesService
    {
        private readonly IAdoptantesRepository _repository;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public AdoptantesService(IAdoptantesRepository repository, IMapper mapper, ApplicationDbContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<AdoptanteDto?> AdoptantesServiceGetByUuid(Guid userUuid)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Uuid == userUuid);
            if (usuario == null)
                return null;

            var adoptante = await _repository.AdoptantesRepositoryGetById(usuario.Id);
            if (adoptante == null)
                return null;

            return _mapper.Map<AdoptanteDto>(adoptante);
        }



        public async Task<object> AdoptantesServiceGetAll(int pageNumber, int pageSize)
        {
            pageSize = pageSize > 12 ? 12 : pageSize;
            pageNumber = pageNumber < 1 ? 1 : pageNumber;

            var (adoptantes, total) = await _repository.AdoptantesRepositoryGetAll(
                pageNumber,
                pageSize);

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

        public async Task<bool> AdoptantesServiceUpdate(Guid id, AdoptanteDto dto)
        {
            var entidad = await _repository.AdoptantesRepositoryGetByUuid(id);
            if (entidad == null)
                return false;

            _mapper.Map(dto, entidad);
            var ok = await _repository.AdoptantesRepositoryUpdate(entidad);
            await _context.SaveChangesAsync();

            return ok;
        }

        public async Task<AdoptanteDto> AdoptantesServiceCreate(AdoptanteDto dto)
        {
            var entidad = _mapper.Map<Adoptante>(dto);
            var creado = await _repository.AdoptantesRepositoryCreate(entidad);
            await _context.SaveChangesAsync();
            return _mapper.Map<AdoptanteDto>(creado);
        }
    }
}
