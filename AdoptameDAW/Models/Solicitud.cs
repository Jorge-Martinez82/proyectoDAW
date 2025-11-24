using System;

namespace AdoptameDAW.Models
{
    public class Solicitud
    {
        public int Id { get; set; }
        public int UsuarioAdoptanteId { get; set; }
        public int UsuarioProtectoraId { get; set; }
        public int AnimalId { get; set; }
        public string Estado { get; set; } = "pendiente"; 
        public DateTime CreatedAt { get; set; }
        public string? Comentario { get; set; } 

        public Usuario UsuarioAdoptante { get; set; } = null!;
        public Usuario UsuarioProtectora { get; set; } = null!;
        public Animal Animal { get; set; } = null!;
    }
}