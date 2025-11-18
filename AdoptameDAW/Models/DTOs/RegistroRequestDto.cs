namespace AdoptameDAW.Models.DTOs
{
    public class RegistroRequestDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string TipoUsuario { get; set; } = null!;
    }
}
