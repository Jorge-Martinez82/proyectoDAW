namespace AdoptameDAW.Models.DTOs
{
    public class RegistroRequestDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string TipoUsuario { get; set; } = null!;

        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }
        public string? Direccion { get; set; }
        public string? CodigoPostal { get; set; }
        public string? Poblacion { get; set; }
        public string? Provincia { get; set; }
        public string? Telefono { get; set; }

        public string? NombreProtectora { get; set; }
    }
}
