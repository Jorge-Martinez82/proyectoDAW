namespace AdoptameDAW.Models.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = null!;
        public UsuarioDto Usuario { get; set; } = null!;
    }
}
