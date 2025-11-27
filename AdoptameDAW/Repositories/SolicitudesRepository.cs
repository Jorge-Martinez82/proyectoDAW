using AdoptameDAW.Data;
using AdoptameDAW.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AdoptameDAW.Repositories
{
    public class SolicitudesRepository : ISolicitudesRepository
    {
        private readonly ApplicationDbContext _context;

        public SolicitudesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // metodo que crea una nueva solicitud
        public async Task<Solicitud> CreateAsync(Solicitud solicitud)
        {
            solicitud.CreatedAt = DateTime.UtcNow;
            await _context.Solicitudes.AddAsync(solicitud);
            await _context.SaveChangesAsync();
            return await _context.Solicitudes
                .Include(s => s.Animal)
                .Include(s => s.UsuarioAdoptante)
                .Include(s => s.UsuarioProtectora)
                .FirstAsync(s => s.Id == solicitud.Id);
        }

        // metodo que obtiene una solicitud por id
        public async Task<Solicitud?> GetByIdAsync(int id)
        {
            return await _context.Solicitudes
                .Include(s => s.Animal)
                .Include(s => s.UsuarioAdoptante)
                .Include(s => s.UsuarioProtectora)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        // metodo que obtiene solicitudes por adoptante con paginacion
        public async Task<(IEnumerable<Solicitud> solicitudes, int total)> GetByAdoptanteAsync(Guid usuarioAdoptanteUuid, int pageNumber, int pageSize)
        {
            var usuario = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Uuid == usuarioAdoptanteUuid);
            if (usuario == null)
                return (Enumerable.Empty<Solicitud>(), 0);

            var query = _context.Solicitudes
                .Include(s => s.Animal)
                .Include(s => s.UsuarioAdoptante)
                .Include(s => s.UsuarioProtectora)
                .Where(s => s.UsuarioAdoptanteId == usuario.Id)
                .AsNoTracking()
                .OrderBy(s => s.Id);

            var total = await query.CountAsync();
            var solicitudes = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (solicitudes, total);
        }

        // metodo que obtiene solicitudes por protectora con paginacion
        public async Task<(IEnumerable<Solicitud> solicitudes, int total)> GetByProtectoraAsync(Guid usuarioProtectoraUuid, int pageNumber, int pageSize)
        {
            var usuario = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Uuid == usuarioProtectoraUuid);
            if (usuario == null)
                return (Enumerable.Empty<Solicitud>(), 0);

            var query = _context.Solicitudes
                .Include(s => s.Animal)
                .Include(s => s.UsuarioAdoptante)
                .Include(s => s.UsuarioProtectora)
                .Where(s => s.UsuarioProtectoraId == usuario.Id)
                .AsNoTracking()
                .OrderBy(s => s.Id);

            var total = await query.CountAsync();
            var solicitudes = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (solicitudes, total);
        }

        // metodo que actualiza el estado de una solicitud
        public async Task<bool> UpdateEstadoAsync(int solicitudId, string nuevoEstado, int usuarioProtectoraId)
        {
            var entidad = await _context.Solicitudes.FirstOrDefaultAsync(s => s.Id == solicitudId && s.UsuarioProtectoraId == usuarioProtectoraId);
            if (entidad == null) return false;

            entidad.Estado = nuevoEstado;
            _context.Solicitudes.Update(entidad);
            var cambios = await _context.SaveChangesAsync();
            return cambios > 0;
        }
    }
}