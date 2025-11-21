using AdoptameDAW.Data;
using AdoptameDAW.Models;
using Microsoft.EntityFrameworkCore;
using AdoptameDAW.Repositories;

namespace AdoptameDAW.Repository
{
    public class AdoptantesRepository : IAdoptantesRepository
    {
        private readonly ApplicationDbContext _context;

        public AdoptantesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Adoptante?> AdoptantesRepositoryGetByUuid(Guid id)
        {
            return await _context.Adoptantes
                .Include(a => a.Usuario)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Uuid == id);
        }

        public async Task<Adoptante?> AdoptantesRepositoryGetById(int userId)
        {
            return await _context.Adoptantes
                .Include(a => a.Usuario)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.UserId == userId);
        }

        public async Task<(IEnumerable<Adoptante> adoptantes, int total)> AdoptantesRepositoryGetAll(
            int pageNumber,
            int pageSize)
        {
            var query = _context.Adoptantes
                .Include(a => a.Usuario)
                .AsNoTracking();

            var total = await query.CountAsync();

            var adoptantes = await query
                .OrderBy(a => a.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (adoptantes, total);
        }

        public async Task<Adoptante> AdoptantesRepositoryCreate(Adoptante adoptante)
        {
            adoptante.Uuid = Guid.NewGuid();
            adoptante.CreatedAt = DateTime.UtcNow;

            await _context.Adoptantes.AddAsync(adoptante);

            return adoptante; 
        }

        public async Task<bool> AdoptantesRepositoryUpdate(Adoptante adoptante)
        {
            var entidad = await _context.Adoptantes
                .FirstOrDefaultAsync(a => a.Uuid == adoptante.Uuid);

            if (entidad == null)
                return false;

            entidad.Nombre = adoptante.Nombre;
            entidad.Apellidos = adoptante.Apellidos;
            entidad.Direccion = adoptante.Direccion;
            entidad.CodigoPostal = adoptante.CodigoPostal;
            entidad.Poblacion = adoptante.Poblacion;
            entidad.Provincia = adoptante.Provincia;
            entidad.Telefono = adoptante.Telefono;
            entidad.Email = adoptante.Email;

            _context.Adoptantes.Update(entidad);

            return true;
        }
    }
}
