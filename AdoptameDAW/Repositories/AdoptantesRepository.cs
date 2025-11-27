using AdoptameDAW.Data;
using AdoptameDAW.Models;
using Microsoft.EntityFrameworkCore;

namespace AdoptameDAW.Repositories
{
    public class AdoptantesRepository : IAdoptantesRepository
    {
        private readonly ApplicationDbContext _context;

        public AdoptantesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // metodo que obtiene un adoptante por uuid
        public async Task<Adoptante?> GetByUuidAsync(Guid adoptanteUuid)
        {
            return await _context.Adoptantes
                .Include(a => a.Usuario)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Uuid == adoptanteUuid);
        }

        // metodo que obtiene un adoptante por id
        public async Task<Adoptante?> GetByIdAsync(int id)
        {
            return await _context.Adoptantes
                .Include(a => a.Usuario)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        // metodo que obtiene el perfil de adoptante por uuid de usuario
        public async Task<Adoptante?> GetByUsuarioUuidAsync(Guid usuarioUuid)
        {
            var usuario = await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Uuid == usuarioUuid);
            if (usuario == null) return null;

            return await _context.Adoptantes
                .Include(a => a.Usuario)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.UserId == usuario.Id);
        }

        // metodo que devuelve adoptantes con paginacion
        public async Task<(IEnumerable<Adoptante> adoptantes, int total)> GetAllAsync(int pageNumber, int pageSize)
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

        // metodo que crea un nuevo adoptante
        public async Task<Adoptante> CreateAsync(Adoptante adoptante)
        {
            adoptante.Uuid = Guid.NewGuid();
            adoptante.CreatedAt = DateTime.UtcNow;
            await _context.Adoptantes.AddAsync(adoptante);
            await _context.SaveChangesAsync();
            return adoptante;
        }

        // metodo que actualiza un adoptante y su usuario asociado
        public async Task<bool> UpdateAsync(Adoptante adoptante)
        {
            var entidad = await _context.Adoptantes
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(a => a.Uuid == adoptante.Uuid);
            if (entidad == null) return false;

            entidad.Nombre = adoptante.Nombre;
            entidad.Apellidos = adoptante.Apellidos;
            entidad.Direccion = adoptante.Direccion;
            entidad.CodigoPostal = adoptante.CodigoPostal;
            entidad.Poblacion = adoptante.Poblacion;
            entidad.Provincia = adoptante.Provincia;
            entidad.Telefono = adoptante.Telefono;
            entidad.Email = adoptante.Email;

            if (entidad.Usuario != null && entidad.Usuario.Email != adoptante.Email)
            {
                entidad.Usuario.Email = adoptante.Email;
                _context.Usuarios.Update(entidad.Usuario);
            }

            _context.Adoptantes.Update(entidad);
            var cambios = await _context.SaveChangesAsync();
            return cambios > 0;
        }
    }
}
