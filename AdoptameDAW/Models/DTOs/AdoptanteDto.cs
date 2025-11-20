namespace AdoptameDAW.Models.DTOs;

public class AdoptanteDto
{
    public Guid Uuid { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string? Direccion { get; set; }

    public string? CodigoPostal { get; set; }

    public string? Poblacion { get; set; }

    public string? Provincia { get; set; }

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; }
}
