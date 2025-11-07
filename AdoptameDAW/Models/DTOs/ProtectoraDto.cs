namespace AdoptameDAW.Models.DTOs;

public class ProtectoraDto
{

    public Guid Uuid { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public string? Provincia { get; set; }

    public string? Email { get; set; }

    public string? Imagen { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; }
}
